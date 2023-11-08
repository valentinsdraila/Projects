import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Book } from '../book';
import { BookListService } from '../booklist.service';

import { MatDialog } from '@angular/material/dialog'
import { AddNewBookComponent } from '../add-new-book/add-new-book.component';

import { ChangeDetectorRef } from '@angular/core';
import { MatSnackBar, _SnackBarContainer } from '@angular/material/snack-bar';
import { DeleteBookComponent } from '../delete-book/delete-book.component';
import { SearchBookComponent } from '../search-book/search-book.component';
@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.scss']
})

export class MainPageComponent implements OnInit {

  public bookListTable: Book[] = [];
  displayedColumns: string[] = ['title', 'author', 'year', 'description', 'rating'];

  constructor(private tableService: BookListService, private router: Router,
    private _snackBar : MatSnackBar,
    private changeDetectorRefs: ChangeDetectorRef, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.bookListTable = this.tableService.getTableList();
  }

  getTableData() {
    this.bookListTable = this.tableService.getTableList();
  }

  clearData() {
    this.bookListTable = [];
  }

   logout() {
     window.localStorage.removeItem("token");

     this.router.navigateByUrl('/auth');
     this._snackBar.open('Log out successfully!', '', {
       duration: 2000,
     })
   }

  

  sortByRating () {
    this.tableService.sortByRating();
    this.getTableData();
  }

  refreshTable () {
    this.getTableData();
    for (const u of this.bookListTable)
    {
      console.log(u.author);
    }
  }

  deleteBook () {
    this.dialog.open(DeleteBookComponent).afterClosed().subscribe(result => {
      this.refresh();
    });
  }

  addBook () {
    this.dialog.open(AddNewBookComponent).afterClosed().subscribe(result => {
      this.refresh();
      this.getTableData();
    });
  }

  refresh() {
      this.changeDetectorRefs.detectChanges();
    }

  searchBook() {
    this.dialog.open(SearchBookComponent).afterClosed().subscribe(result => {
      this.refresh();
      this.getTableData();
    })
  }
}
