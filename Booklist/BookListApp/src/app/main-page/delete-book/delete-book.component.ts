import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BookListService } from '../booklist.service';

@Component({
  selector: 'app-delete-book',
  templateUrl: './delete-book.component.html',
  styleUrls: ['./delete-book.component.scss']
})
export class DeleteBookComponent implements OnInit {
  form! : FormGroup;

  constructor(private service : BookListService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.form = new FormGroup ({
      bookTitle: new FormControl(null, Validators.required)
    });
  }


  deleteBook() {
    let title : string = this.getTitle?.value;

    this.service.deleteBook(title);
  }

  get getTitle() {
    return this.form.get("bookTitle");
  }
}
