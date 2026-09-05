import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { EvenementService } from '../../core/services/evenement.service';
import { EvenementDTO, TypeEvenement } from '../../core/models/evenement.model';
import { WeekCalendarComponent } from '../../shared/week-calendar/week-calendar';

@Component({
  selector: 'app-accueil',
  imports: [WeekCalendarComponent],
  templateUrl: './accueil.html'
})
export class AccueilComponent implements OnInit {
  private evenementService = inject(EvenementService);

  private allEvents = signal<EvenementDTO[]>([]);
  private activeTypes = signal<Set<TypeEvenement>>(new Set(['cours', 'examen', 'salle']));

  readonly filteredEvents = computed(() => this.allEvents().filter((e) => this.activeTypes().has(e.type)));

  readonly chipTypes: TypeEvenement[] = ['cours', 'examen', 'salle'];
  readonly chipLabels: Record<TypeEvenement, string> = { cours: 'Cours', examen: 'Examens', salle: 'Salle' };

  ngOnInit(): void {
    this.load();
  }

  isActive(type: TypeEvenement): boolean {
    return this.activeTypes().has(type);
  }

  toggleType(type: TypeEvenement): void {
    const next = new Set(this.activeTypes());
    if (next.has(type)) next.delete(type);
    else next.add(type);
    this.activeTypes.set(next);
  }

  onEventClick(evt: EvenementDTO): void {
    const detail = `${evt.titre}\n${evt.heureDebut.slice(0, 5)}–${evt.heureFin.slice(0, 5)}` + (evt.commentaire ? `\n${evt.commentaire}` : '');
    if (confirm(`${detail}\n\nSupprimer cet événement ?`)) {
      this.evenementService.delete(evt.idEvenement).subscribe(() => this.load());
    }
  }

  private load(): void {
    this.evenementService.getAll().subscribe((events) => this.allEvents.set(events));
  }
}
