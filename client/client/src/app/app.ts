import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BooksComponent } from './books/books.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, BooksComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('client');
}
