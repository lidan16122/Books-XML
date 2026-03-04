export interface Book {
  isbn: string;
  title: string;
  category: string;
  cover?: string | null;
  lang: string;
  authors: string[];
  year: number;
  price: number;
}
