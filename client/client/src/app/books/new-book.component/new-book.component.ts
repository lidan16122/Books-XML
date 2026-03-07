import { Component, inject, output } from '@angular/core';
import { Input } from '@angular/core';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Book } from '../books.models';
import { BooksService } from '../books.service';

@Component({
  selector: 'app-new-book',
  imports: [ReactiveFormsModule],
  templateUrl: './new-book.component.html',
  styleUrl: './new-book.component.css',
})
export class NewBookComponent {
  private fb = inject(FormBuilder);
  private bookService = inject(BooksService);
  cancel = output<boolean>();
  submitEdit = output<Book>();
  @Input() book?: Book;
  @Input() editMode = false;

  form = this.fb.group({
    isbn: ['', [Validators.required]],
    title: ['', [Validators.required, Validators.minLength(2)]],
    category: ['', Validators.required],
    language: ['', Validators.required],
    cover: [null as string | null],
    year: [null as number | null, [Validators.required, Validators.min(1000), Validators.max(2026)]],
    price: [null as number | null, [Validators.required, Validators.min(0)]],
    authors: this.fb.array([
      this.fb.control('', Validators.required)  // starts with one author field
    ])
  });

  ngOnInit() {
    if (this.book) {
      this.editMode = true;
      this.form.patchValue({
        isbn: this.book.isbn,
        title: this.book.title,
        category: this.book.category,
        language: this.book.lang,
        cover: this.book.cover ?? '',
        year: this.book.year,
        price: this.book.price,
      });
      this.authors.clear();
      this.book.authors.forEach(a => this.authors.push(this.fb.control(a, Validators.required)));
    }
  }

  get isbn() { return this.form.get('isbn'); }
  get title() { return this.form.get('title'); }
  get category() { return this.form.get('category'); }
  get language() { return this.form.get('language'); }
  get cover() { return this.form.get('cover'); }
  get year() { return this.form.get('year'); }
  get price() { return this.form.get('price'); }

  get authors(): FormArray {
    return this.form.get('authors') as FormArray;
  }

  addAuthor() {
    this.authors.push(this.fb.control('', Validators.required));
  }

  removeAuthor(index: number) {
    if (this.authors.length > 1) {
      this.authors.removeAt(index);
    }
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const book: Book = {
      isbn: this.form.value.isbn!,
      title: this.form.value.title!,
      category: this.form.value.category!,
      lang: this.form.value.language!,
      cover: this.form.value.cover ?? null,
      year: this.form.value.year!,
      price: this.form.value.price!,
      authors: this.form.value.authors!.filter(Boolean) as string[],
    };
    if (this.editMode) {
      this.submitEdit.emit(book);
      this.onCancel();
    } else {
      this.bookService.createBook(book).subscribe({
        next: (result) => {
          if (result) {
            this.onCancel();
          }
        },
        error: (err) => {
          console.error('Error creating book:', err);
        }
      });
    }
  }
  onCancel() {
    this.cancel.emit(false);
  }
}
