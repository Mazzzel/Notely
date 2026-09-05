export interface SerieDTO {
  idSerie: number;
  numeroSerie: number;
  nombreReps: number;
  poids: number | null;
}

export interface SerieCreateDTO {
  idExerciceSeance: number;
  numeroSerie: number;
  nombreReps: number;
  poids?: number | null;
}

export interface SerieUpdateDTO {
  numeroSerie: number;
  nombreReps: number;
  poids?: number | null;
}

export interface ExerciceSeanceDTO {
  idExerciceSeance: number;
  idSeance: number;
  nom: string;
  series: SerieDTO[];
}

export interface ExerciceSeanceCreateDTO {
  idSeance: number;
  nom: string;
}

export interface SeanceDTO {
  idSeance: number;
  date: string;
  commentaire: string | null;
  exercices: ExerciceSeanceDTO[];
}

export interface SeanceCreateDTO {
  date: string;
  commentaire?: string | null;
}

export interface SeanceUpdateDTO {
  date: string;
  commentaire?: string | null;
}

export interface ProgressionPointDTO {
  date: string;
  poidsMax: number | null;
}
