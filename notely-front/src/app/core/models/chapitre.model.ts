export type EtatChapitre = 'non_appris' | 'en_cours' | 'appris';
export type Difficulte = 'facile' | 'moyen' | 'difficile';

export interface ChapitreDTO {
  idChapitre: number;
  idCours: number;
  libelle: string;
  etat: EtatChapitre;
  date: string | null;
  difficulte: Difficulte;
}

export interface ChapitreCreateDTO {
  idCours: number;
  libelle: string;
  etat: EtatChapitre;
  date?: string | null;
  difficulte: Difficulte;
}

export interface ChapitreUpdateDTO {
  libelle: string;
  etat: EtatChapitre;
  date?: string | null;
  difficulte: Difficulte;
}
