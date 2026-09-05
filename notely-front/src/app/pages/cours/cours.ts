import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { CoursService } from '../../core/services/cours.service';
import { TodoService } from '../../core/services/todo.service';
import { NoteService } from '../../core/services/note.service';
import { EvenementService } from '../../core/services/evenement.service';
import { CoursDTO } from '../../core/models/cours.model';
import { TodoDTO } from '../../core/models/todo.model';
import { NoteDTO } from '../../core/models/note.model';
import { EvenementDTO, TypeEvenement } from '../../core/models/evenement.model';
import { WeekCalendarComponent } from '../../shared/week-calendar/week-calendar';
import { CourseModalComponent } from '../../shared/course-modal/course-modal';

function endAfterStart(): ValidatorFn {
  return (group): ValidationErrors | null => {
    const start = group.get('heureDebut')?.value;
    const end = group.get('heureFin')?.value;
    if (!start || !end) return null;
    return end > start ? null : { endBeforeStart: true };
  };
}

function toLocalISODate(d: Date): string {
  const y = d.getFullYear();
  const m = String(d.getMonth() + 1).padStart(2, '0');
  const day = String(d.getDate()).padStart(2, '0');
  return `${y}-${m}-${day}`;
}

const TAB_COLORS = ['#2f6f63', '#c98a2e', '#a8503f', '#3d6ea5', '#7a5ea8'];
const DEFAULT_COLORS: Record<TypeEvenement, string> = { cours: '#3d6ea5', examen: '#a8503f', salle: '#7a5ea8' };

@Component({
  selector: 'app-cours',
  imports: [FormsModule, ReactiveFormsModule, WeekCalendarComponent, CourseModalComponent],
  templateUrl: './cours.html',
  styleUrl: './cours.scss'
})
export class CoursComponent implements OnInit {
  private coursService = inject(CoursService);
  private todoService = inject(TodoService);
  private noteService = inject(NoteService);
  private evenementService = inject(EvenementService);
  private fb = inject(FormBuilder);

  readonly coursList = signal<CoursDTO[]>([]);
  readonly todos = signal<TodoDTO[]>([]);
  readonly notes = signal<NoteDTO[]>([]);
  private allEvents = signal<EvenementDTO[]>([]);
  private activeTypes = signal<Set<TypeEvenement>>(new Set(['cours', 'examen']));

  readonly selectedCoursId = signal<number | null>(null);

  readonly sortedTodos = computed(() =>
    [...this.todos()].sort((a, b) => (a.date ?? '9999').localeCompare(b.date ?? '9999'))
  );

  readonly filteredEvents = computed(() => this.allEvents().filter((e) => this.activeTypes().has(e.type)));

  readonly examList = computed(() =>
    this.allEvents()
      .filter((e) => e.type === 'examen')
      .sort((a, b) => `${a.date}T${a.heureDebut}`.localeCompare(`${b.date}T${b.heureDebut}`))
  );

  readonly chipTypes: TypeEvenement[] = ['cours', 'examen'];
  readonly chipLabels: Record<TypeEvenement, string> = { cours: 'Cours', examen: 'Examens', salle: 'Salle' };

  private colorTouched = false;

  newTodoNom = '';
  newTodoCoursId: number | null = null;
  newTodoDate = '';
  newNoteTexte = '';
  newCourseNom = '';

  eventForm = this.fb.nonNullable.group(
    {
      type: 'cours' as TypeEvenement,
      titre: ['', Validators.required],
      date: [toLocalISODate(new Date()), Validators.required],
      heureDebut: ['', Validators.required],
      heureFin: ['', Validators.required],
      couleur: ['#3d6ea5', Validators.required],
      commentaire: ['']
    },
    { validators: endAfterStart() }
  );

  ngOnInit(): void {
    this.loadCours();
    this.loadTodos();
    this.loadNotes();
    this.loadEvents();

    this.eventForm.get('type')!.valueChanges.subscribe((type) => {
      if (!this.colorTouched) this.eventForm.get('couleur')!.setValue(DEFAULT_COLORS[type]);
    });
  }

  readonly ringRadius = 20;
  readonly ringCircumference = 2 * Math.PI * this.ringRadius;

  ringOffset(pct: number): number {
    return this.ringCircumference - (pct / 100) * this.ringCircumference;
  }

  tabColor(index: number): string {
    return TAB_COLORS[index % TAB_COLORS.length];
  }

  progressPct(cours: CoursDTO): number {
    return cours.nombreChapitres ? Math.round((cours.nombreChapitresAppris / cours.nombreChapitres) * 100) : 0;
  }

  courseName(idCours: number): string {
    return this.coursList().find((c) => c.idCours === idCours)?.nom ?? '—';
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

  onColorInput(): void {
    this.colorTouched = true;
  }

  toggleTodoFait(todo: TodoDTO): void {
    this.todoService.update(todo.idTodo, { nom: todo.nom, fait: !todo.fait, date: todo.date }).subscribe(() => {
      this.loadTodos();
      this.loadCours();
    });
  }

  deleteTodo(id: number): void {
    this.todoService.delete(id).subscribe(() => {
      this.loadTodos();
      this.loadCours();
    });
  }

  addTodo(): void {
    const nom = this.newTodoNom.trim();
    if (!nom || !this.newTodoCoursId) return;

    this.todoService.create({ nom, idCours: this.newTodoCoursId, date: this.newTodoDate || null }).subscribe(() => {
      this.newTodoNom = '';
      this.newTodoDate = '';
      this.loadTodos();
      this.loadCours();
    });
  }

  toggleNoteFait(note: NoteDTO): void {
    this.noteService.update(note.idNote, { texte: note.texte, fait: !note.fait }).subscribe(() => this.loadNotes());
  }

  deleteNote(id: number): void {
    this.noteService.delete(id).subscribe(() => this.loadNotes());
  }

  addNote(): void {
    const texte = this.newNoteTexte.trim();
    if (!texte) return;

    this.noteService.create({ texte }).subscribe(() => {
      this.newNoteTexte = '';
      this.loadNotes();
    });
  }

  addCourse(): void {
    const nom = this.newCourseNom.trim();
    if (!nom) return;

    this.coursService.create({ nom }).subscribe(() => {
      this.newCourseNom = '';
      this.loadCours();
    });
  }

  openCourseModal(idCours: number): void {
    this.selectedCoursId.set(idCours);
  }

  closeCourseModal(): void {
    this.selectedCoursId.set(null);
  }

  onModalChanged(): void {
    this.loadCours();
    this.loadTodos();
  }

  onEventClick(evt: EvenementDTO): void {
    const detail = `${evt.titre}\n${evt.heureDebut.slice(0, 5)}–${evt.heureFin.slice(0, 5)}` + (evt.commentaire ? `\n${evt.commentaire}` : '');
    if (confirm(`${detail}\n\nSupprimer cet événement ?`)) {
      this.evenementService.delete(evt.idEvenement).subscribe(() => this.loadEvents());
    }
  }

  submitEvent(): void {
    if (this.eventForm.invalid) return;

    const value = this.eventForm.getRawValue();
    this.evenementService
      .create({
        type: value.type,
        titre: value.titre,
        couleur: value.couleur,
        date: value.date,
        heureDebut: value.heureDebut,
        heureFin: value.heureFin,
        commentaire: value.commentaire || null
      })
      .subscribe(() => {
        this.eventForm.patchValue({ titre: '', heureDebut: '', heureFin: '', commentaire: '' });
        this.loadEvents();
      });
  }

  private loadCours(): void {
    this.coursService.getAll().subscribe((list) => this.coursList.set(list));
  }

  private loadTodos(): void {
    this.todoService.getAll().subscribe((list) => this.todos.set(list));
  }

  private loadNotes(): void {
    this.noteService.getAll().subscribe((list) => this.notes.set(list));
  }

  private loadEvents(): void {
    this.evenementService.getAll().subscribe((list) => this.allEvents.set(list));
  }
}
