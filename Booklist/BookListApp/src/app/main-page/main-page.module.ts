import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainPageComponent } from './main-page/main-page.component';
import { MainPageRoutingModule } from './main-page-routing-module';
import {MatTableModule} from '@angular/material/table';
import {MatButtonModule} from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddNewBookComponent } from './add-new-book/add-new-book.component';

import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { DeleteBookComponent } from './delete-book/delete-book.component';
import { SearchBookComponent } from './search-book/search-book.component';


@NgModule({
  declarations: [
    MainPageComponent,
    AddNewBookComponent,
    DeleteBookComponent,
    SearchBookComponent,
  ],
  imports: [
    CommonModule,
    MainPageRoutingModule,
    MatTableModule,
    MatButtonModule,
    MatDialogModule,
    FormsModule,
    ReactiveFormsModule,

    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule,
    
  ]
})
export class MainPageModule { }
