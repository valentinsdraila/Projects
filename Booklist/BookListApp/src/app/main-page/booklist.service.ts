import { Injectable } from '@angular/core';
import { Book } from './book';

import { Observable } from 'rxjs';

import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class BookListService {

    private membersUrl = 'http://localhost:44340/api/books';

    private tableList: Book[]=[];

    constructor(private httpClient: HttpClient) { }

    getTableList(): Book[] {
        let books : Observable<Book[]> = this.getBooks();

        
        books.forEach(b => this.setBook(b));

        return this.tableList;
    }

    addBook(book : Book) {
        // send book to backend

        this.tableList.push(book);
        for (const b of this.tableList)
            console.log(b.author);
        const url = `${this.membersUrl}/add`; 
    
        return this.httpClient.post(url, book).subscribe();
        
    }

    deleteBook(title : string) {
        // api/books/delete/book

        const url = `${this.membersUrl}/delete/book`;

        let param = new HttpParams().set('title', title);
        return this.httpClient.get(url, {params : param}).subscribe();
    }

    getBooks(): Observable<Book[]> {
        // api/books
        const url = `${this.membersUrl}/get/all`;
        return this.httpClient.get<Book[]>(url);
    }

    setBook(booktable:Book[]):void{
        this.tableList = booktable;
      }

    private bookCompareByRating(book1:Book, book2:Book) {
        return book1.rating - book2.rating;
    }
    sortByRating(){
        this.tableList.sort(this.bookCompareByRating).reverse(); // rating compare by difference
    }

    searchBook(title : string) {
        return this.tableList.filter(book => book.title.includes(title));
    }
}