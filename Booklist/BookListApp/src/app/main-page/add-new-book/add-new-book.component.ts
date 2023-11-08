import { Component, OnInit } from '@angular/core';

import { MatDialog } from '@angular/material/dialog';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BookListService } from '../booklist.service';
import { Book } from '../book';
import { MainPageComponent } from '../main-page/main-page.component';

@Component({
  selector: 'app-add-new-book',
  templateUrl: './add-new-book.component.html',
  styleUrls: ['./add-new-book.component.scss']
})
export class AddNewBookComponent implements OnInit {
  form!: FormGroup;

  constructor(private service : BookListService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.form = new FormGroup({
      title: new FormControl(null, Validators.required),
      year: new FormControl(null, Validators.required),
      description: new FormControl(null, Validators.required),
      rating: new FormControl(null, Validators.required),
      author: new FormControl(null, Validators.required)
    })
  }

  addBook() {
      let book = new Book(this.getTitle?.value, this.getAuthor?.value,
        this.getYear?.value, this.getDescription?.value, this.getRating?.value);
      this.service.addBook(book);
     
  }

  get getTitle() {
    return this.form.get('title');
  }

  get getYear() {
    return this.form.get('year');
  }

  get getAuthor() {
    return this.form.get('author');
  }

  get getDescription() {
    return this.form.get('description');
  }

  get getRating() {
    return this.form.get('rating');
  }
}
