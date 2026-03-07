import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Book } from './books.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BooksService {
  apiRoute = `${environment.apiUrl}api/books`;

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
