import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators, FormBuilder } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { EvenementService } from '../../core/services/evenement.service';
import { SeanceService } from '../../core/services/seance.service';
import { ExerciceSeanceService } from '../../core/services/exercice-seance.service';
import { SerieService } from '../../core/services/serie.service';
import { EvenementDTO } from '../../core/models/evenement.model';
import { ProgressionPointDTO, SeanceDTO } from '../../core/models/seance.model';
import { WeekCalendarComponent } from '../../shared/week-calendar/week-calendar';
import { ProgressionChartComponent } from '../../shared/progression-chart/progression-chart';

interface DraftSerie {
  numeroSerie: number;
  nombreReps: number;
  poids: number | null;
}

interface DraftExercice {
  nom: string;
  series: DraftSerie[];
  nextReps: number | null;
  nextPoids: number | null;
}

function toLocalISODate(d: Date): string {
  const y = d.getFullYear();
  const m = String(d.getMonth() + 1).padStart(2, '0');
  const day = String(d.getDate()).padStart(2, '0');
  return `${y}-${m}-${day}`;
}

function endAfterStart(): ValidatorFn {
  return (group): ValidationErrors | null => {
    const start = group.get('heureDebut')?.value;
    const end = group.get('heureFin')?.value;
    if (!start || !end) return null;
    return end > start ? null : { endBeforeStart: true };
  };
}

@Component({
  selector: 'app-salle',
  imports: [FormsModule, ReactiveFormsModule, WeekCalendarComponent, ProgressionChartComponent],
  templateUrl: './salle.html',
  styleUrl: './salle.scss'
})
export class SalleComponent implements OnInit {
  private evenementService = inject(EvenementService);
  private seanceService = inject(SeanceService);
  private exerciceSeanceService = inject(ExerciceSeanceService);
  private serieService = inject(SerieService);
  private fb = inject(FormBuilder);

  readonly events = signal<EvenementDTO[]>([]);
  readonly errorMessage = signal<string | null>(null);

  readonly seances = signal<SeanceDTO[]>([]);
  readonly exerciceNames = signal<string[]>([]);
  readonly progressionExercice = signal<string | null>(null);
  readonly progressionPoints = signal<ProgressionPointDTO[]>([]);

  draftDate = toLocalISODate(new Date());
  draftCommentaire = '';
  readonly draftExercices = signal<DraftExercice[]>([]);
  newExerciceNom = '';
  readonly savingSeance = signal(false);
  readonly seanceError = signal<string | null>(null);

  form = this.fb.nonNullable.group(
    {
      titre: ['', Validators.required],
      date: [toLocalISODate(new Date()), Validators.required],
      heureDebut: ['', Validators.required],
      heureFin: ['', Validators.required],
      couleur: ['#7a5ea8', Validators.required],
      commentaire: ['']
    },
    { validators: endAfterStart() }
  );

  ngOnInit(): void {
    this.load();
    this.loadSeances();
    this.loadExerciceNames();
  }

  onEventClick(evt: EvenementDTO): void {
    const detail = `Salle — ${evt.titre}\n${evt.heureDebut.slice(0, 5)}–${evt.heureFin.slice(0, 5)}` + (evt.commentaire ? `\n${evt.commentaire}` : '');
    if (confirm(`${detail}\n\nSupprimer cet événement ?`)) {
      this.evenementService.delete(evt.idEvenement).subscribe(() => this.load());
    }
  }

  submit(): void {
    if (this.form.invalid) return;
    this.errorMessage.set(null);

    const value = this.form.getRawValue();
    this.evenementService
      .create({
        type: 'salle',
        titre: value.titre,
        couleur: value.couleur,
        date: value.date,
        heureDebut: value.heureDebut,
        heureFin: value.heureFin,
        commentaire: value.commentaire || null
      })
      .subscribe({
        next: () => {
          this.form.patchValue({ titre: '', heureDebut: '', heureFin: '', commentaire: '' });
          this.load();
        },
        error: (err) => this.errorMessage.set(err?.error?.message ?? "Impossible d'ajouter cet événement.")
      });
  }

  addDraftExercice(): void {
    const nom = this.newExerciceNom.trim();
    if (!nom) return;

    this.draftExercices.update((list) => [...list, { nom, series: [], nextReps: null, nextPoids: null }]);
    this.newExerciceNom = '';
  }

  removeDraftExercice(index: number): void {
    this.draftExercices.update((list) => list.filter((_, i) => i !== index));
  }

  addDraftSerie(index: number): void {
    this.draftExercices.update((list) =>
      list.map((ex, i) => {
        if (i !== index || !ex.nextReps) return ex;
        const serie: DraftSerie = { numeroSerie: ex.series.length + 1, nombreReps: ex.nextReps, poids: ex.nextPoids };
        return { ...ex, series: [...ex.series, serie], nextReps: null, nextPoids: null };
      })
    );
  }

  removeDraftSerie(exIndex: number, serieIndex: number): void {
    this.draftExercices.update((list) =>
      list.map((ex, i) => {
        if (i !== exIndex) return ex;
        const series = ex.series.filter((_, si) => si !== serieIndex).map((s, si) => ({ ...s, numeroSerie: si + 1 }));
        return { ...ex, series };
      })
    );
  }

  async saveSeance(): Promise<void> {
    if (this.draftExercices().length === 0) {
      this.seanceError.set('Ajoute au moins un exercice à la séance.');
      return;
    }

    this.savingSeance.set(true);
    this.seanceError.set(null);

    try {
      const seance = await firstValueFrom(
        this.seanceService.create({ date: this.draftDate, commentaire: this.draftCommentaire || null })
      );

      for (const exercice of this.draftExercices()) {
        const created = await firstValueFrom(
          this.exerciceSeanceService.create({ idSeance: seance.idSeance, nom: exercice.nom })
        );

        for (const serie of exercice.series) {
          await firstValueFrom(
            this.serieService.create({
              idExerciceSeance: created.idExerciceSeance,
              numeroSerie: serie.numeroSerie,
              nombreReps: serie.nombreReps,
              poids: serie.poids
            })
          );
        }
      }

      this.draftDate = toLocalISODate(new Date());
      this.draftCommentaire = '';
      this.draftExercices.set([]);
      this.loadSeances();
      this.loadExerciceNames();
    } catch (err: any) {
      this.seanceError.set(err?.error?.message ?? "Erreur lors de l'enregistrement de la séance.");
    } finally {
      this.savingSeance.set(false);
    }
  }

  deleteSeance(id: number): void {
    if (!confirm('Supprimer cette séance et tout son contenu ?')) return;
    this.seanceService.delete(id).subscribe(() => {
      this.loadSeances();
      this.loadExerciceNames();
    });
  }

  selectProgressionExercice(nom: string): void {
    this.progressionExercice.set(nom);
    this.seanceService.getProgression(nom).subscribe((points) => this.progressionPoints.set(points));
  }

  private load(): void {
    this.evenementService.getAll().subscribe((events) => this.events.set(events.filter((e) => e.type === 'salle')));
  }

  private loadSeances(): void {
    this.seanceService.getAll().subscribe((list) => this.seances.set(list));
  }

  private loadExerciceNames(): void {
    this.seanceService.getExercices().subscribe((names) => {
      this.exerciceNames.set(names);
      if (!this.progressionExercice() && names.length > 0) {
        this.selectProgressionExercice(names[0]);
      }
    });
  }
}
