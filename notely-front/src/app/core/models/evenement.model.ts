export type TypeEvenement = 'cours' | 'examen' | 'salle';

export interface EvenementDTO {
  idEvenement: number;
  type: TypeEvenement;
  titre: string;
  couleur: string;
  date: string;
  heureDebut: string;
  heureFin: string;
  commentaire: string | null;
}

export interface EvenementCreateDTO {
  type: TypeEvenement;
  titre: string;
  couleur: string;
  date: string;
  heureDebut: string;
  heureFin: string;
  commentaire?: string | null;
}

export interface EvenementUpdateDTO extends EvenementCreateDTO {
}
