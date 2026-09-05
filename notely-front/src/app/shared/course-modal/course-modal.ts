import { Component, OnInit, inject, input, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CoursService } from '../../core/services/cours.service';
import { ChapitreService } from '../../core/services/chapitre.service';
import { TodoService } from '../../core/services/todo.service';
import { CoursDetailDTO } from '../../core/models/cours.model';
import { ChapitreDTO, Difficulte, EtatChapitre } from '../../core/models/chapitre.model';
import { TodoDTO } from '../../core/models/todo.model';

const DIFF_LABEL: Record<Difficulte, string> = { facile: 'Facile', moyen: 'Moyen', difficile: 'Difficile' };
const ETAT_LABEL: Record<EtatChapitre, string> = { non_appris: 'Non appris', en_cours: 'En cours', appris: 'Appris' };

@Component({
  selector: 'app-course-modal',
  imports: [FormsModule],
  templateUrl: './course-modal.html',
  styleUrl: './course-modal.scss'
})
export class CourseModalComponent implements OnInit {
  idCours = input.required<number>();
  closed = output<void>();
  changed = output<void>();

  private coursService = inject(CoursService);
  private chapitreService = inject(ChapitreService);
  private todoService = inject(TodoService);

  readonly detail = signal<CoursDetailDTO | null>(null);
  readonly diffLabel = DIFF_LABEL;
  readonly etatLabel = ETAT_LABEL;

  titleDraft = '';

  newTodoNom = '';
  newTodoDate = '';

  newChapitreLibelle = '';
  newChapitreEtat: EtatChapitre = 'non_appris';
  newChapitreDate = '';
  newChapitreDifficulte: Difficulte = 'moyen';

  ngOnInit(): void {
    this.load();
  }

  close(): void {
    this.closed.emit();
  }

  onOverlayClick(event: MouseEvent): void {
    if (event.target === event.currentTarget) this.close();
  }

  saveTitle(): void {
    const detail = this.detail();
    const nom = this.titleDraft.trim();
    if (!detail || !nom || nom === detail.nom) return;

    this.coursService.update(detail.idCours, { nom }).subscribe(() => {
      this.load();
      this.changed.emit();
    });
  }

  toggleTodoFait(todo: TodoDTO): void {
    this.todoService.update(todo.idTodo, { nom: todo.nom, fait: !todo.fait, date: todo.date }).subscribe(() => {
      this.load();
      this.changed.emit();
    });
  }

  deleteTodo(id: number): void {
    this.todoService.delete(id).subscribe(() => {
      this.load();
      this.changed.emit();
    });
  }

  addTodo(): void {
    const nom = this.newTodoNom.trim();
    if (!nom) return;

    this.todoService.create({ nom, idCours: this.idCours(), date: this.newTodoDate || null }).subscribe(() => {
      this.newTodoNom = '';
      this.newTodoDate = '';
      this.load();
      this.changed.emit();
    });
  }

  updateChapitreEtat(chapitre: ChapitreDTO, etat: EtatChapitre): void {
    this.patchChapitre(chapitre, { etat });
  }

  updateChapitreDate(chapitre: ChapitreDTO, date: string): void {
    this.patchChapitre(chapitre, { date: date || null });
  }

  updateChapitreDifficulte(chapitre: ChapitreDTO, difficulte: Difficulte): void {
    this.patchChapitre(chapitre, { difficulte });
  }

  private patchChapitre(chapitre: ChapitreDTO, patch: Partial<Pick<ChapitreDTO, 'etat' | 'date' | 'difficulte'>>): void {
    this.chapitreService
      .update(chapitre.idChapitre, {
        libelle: chapitre.libelle,
        etat: chapitre.etat,
        date: chapitre.date,
        difficulte: chapitre.difficulte,
        ...patch
      })
      .subscribe(() => {
        this.load();
        this.changed.emit();
      });
  }

  deleteChapitre(id: number): void {
    this.chapitreService.delete(id).subscribe(() => {
      this.load();
      this.changed.emit();
    });
  }

  addChapitre(): void {
    const libelle = this.newChapitreLibelle.trim();
    if (!libelle) return;

    this.chapitreService
      .create({
        idCours: this.idCours(),
        libelle,
        etat: this.newChapitreEtat,
        date: this.newChapitreDate || null,
        difficulte: this.newChapitreDifficulte
      })
      .subscribe(() => {
        this.newChapitreLibelle = '';
        this.newChapitreDate = '';
        this.newChapitreEtat = 'non_appris';
        this.newChapitreDifficulte = 'moyen';
        this.load();
        this.changed.emit();
      });
  }

  private load(): void {
    this.coursService.getById(this.idCours()).subscribe((d) => {
      this.detail.set(d);
      this.titleDraft = d.nom;
    });
  }
}
