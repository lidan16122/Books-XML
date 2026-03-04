import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Book } from './books.models';

@Injectable({
  providedIn: 'root',
})
export class BooksService {
  apiRoute = 'https://localhost:64266/api/books';

  constructor(private http: HttpClient) { }

  getBooks(pageNumber: number, pageSize: number): Observable<Book[]> {
    return this.http.get<Book[]>(this.apiRoute, {
      params:{
        pageNumber,
        pageSize
      }
    });
  }

  createBook(book: Book): Observable<boolean> {
    return this.http.post<boolean>(this.apiRoute, book);
  }

  updateBook(book: Book): Observable<boolean> {
    return this.http.put<boolean>(this.apiRoute, book);
  }

  deleteBook(isbn: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiRoute}/${isbn}`);
  }
}
