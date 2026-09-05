import { ChapitreDTO } from './chapitre.model';
import { TodoDTO } from './todo.model';

export interface CoursDTO {
  idCours: number;
  nom: string;
  dateCreation: string;
  nombreChapitres: number;
  nombreChapitresAppris: number;
  nombreTachesOuvertes: number;
}

export interface CoursDetailDTO {
  idCours: number;
  nom: string;
  dateCreation: string;
  chapitres: ChapitreDTO[];
  todos: TodoDTO[];
}

export interface CoursCreateDTO {
  nom: string;
}

export interface CoursUpdateDTO {
  nom: string;
}
