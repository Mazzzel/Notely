export interface TodoDTO {
  idTodo: number;
  nom: string;
  idCours: number;
  nomCours: string;
  fait: boolean;
  date: string | null;
}

export interface TodoCreateDTO {
  nom: string;
  idCours: number;
  date?: string | null;
}

export interface TodoUpdateDTO {
  nom: string;
  fait: boolean;
  date?: string | null;
}
