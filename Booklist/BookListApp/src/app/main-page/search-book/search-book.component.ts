import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BookListService } from '../booklist.service';

@Component({
  selector: 'app-search-book',
  templateUrl: './search-book.component.html',
  styleUrls: ['./search-book.component.scss']
})
export class SearchBookComponent implements OnInit {

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

  searchBook() {
      let title : string = this.getTitle?.value;

      this.service.setBook(this.service.searchBook(title));
      
  }

  get getTitle() {
    return this.form.get("bookTitle");
  }
}
