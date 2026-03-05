import { Component, OnInit, signal } from '@angular/core';
import { Book } from './books.models';
import { BooksService } from './books.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { tap } from 'rxjs';

@Component({
  selector: 'app-books',
  imports: [],
  templateUrl: './books.component.html',
  styleUrl: './books.component.css',
})
export class BooksComponent implements OnInit {
  PAGE_SIZE = 10;
  booksList = signal<Book[]>([]);

  constructor(private bookService: BooksService) {}

  ngOnInit(){
    this.InitializeData();
  }

  InitializeData() {
    this.bookService.getBooks(1, this.PAGE_SIZE)
      .pipe(tap({
        next: (books) => {
          this.booksList.set(books);
        },
        error: (err) => {
          console.error('Error fetching books:', err);
        }
      }));
  }

  createBook(book: Book) {
    this.bookService.createBook(book)
      .pipe(
        tap({
          next: (result) => {
            if (result) this.InitializeData();
          },
          error: (err) => {
            console.error('Error creating book:', err);
          }
        }))
  }

  updateBook(book: Book) {
    this.bookService.updateBook(book)
      .pipe(
        tap({
          next: (result) => {
            if (result) this.InitializeData();
          },
          error: (err) => {
            console.error('Error updating book:', err);
          }
        }))
  }

  deleteBook(isbn: string) {
    this.bookService.deleteBook(isbn)
      .pipe(
        tap({
          next: (result) => {
            if (result) this.InitializeData();
          },
          error: (err) => {
            console.error('Error deleting book:', err);
          }
        }))
  }

}
