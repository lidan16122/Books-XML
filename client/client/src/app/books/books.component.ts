import { Component, OnInit, signal } from '@angular/core';
import { Book } from './books.models';
import { BooksService } from './books.service';
import { tap } from 'rxjs';
import { NewBookComponent } from './new-book.component/new-book.component';

@Component({
  selector: 'app-books',
  imports: [NewBookComponent],
  templateUrl: './books.component.html',
  styleUrl: './books.component.css',
})
export class BooksComponent implements OnInit {
  PAGE_SIZE = 10;
  booksList = signal<Book[]>([]);
  showSubmitForm = false;
  editBook: Book | null = null;

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
      }))
      .subscribe();
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
      .subscribe();
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
      .subscribe();
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
      .subscribe();
  }
  showSubmitFormToggle(show: boolean) {
    this.showSubmitForm = show;
    this.editBook = null;
    this.InitializeData();
  }

  showEditForm(book: Book) {
    this.editBook = { ...book };
    this.showSubmitForm = false;
  }

  closeEditForm() {
    this.editBook = null;
    this.InitializeData();
  }
}
