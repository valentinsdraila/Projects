export class Book{
    title: string;
    author: string;
    year: number;
    description: string;
    rating: number;

    constructor(title:string,author:string, year:number, description:string, rating:number)
    {
        this.title = title;
        this.author = author;
        this.year = year;
        this.description = description;
        this.rating = rating;
    }
}