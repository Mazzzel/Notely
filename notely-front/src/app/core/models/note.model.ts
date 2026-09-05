export interface NoteDTO {
  idNote: number;
  texte: string;
  fait: boolean;
}

export interface NoteCreateDTO {
  texte: string;
}

export interface NoteUpdateDTO {
  texte: string;
  fait: boolean;
}
