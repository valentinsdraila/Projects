#include "DatabaseConnection.h"

#include <iostream>

DatabaseConnection::DatabaseConnection() :
    conninfo("dbname = Twitter")
{
    conn = PQconnectdb("user=postgres password=1q2w3e dbname=Twitter hostaddr=127.0.0.1 port=5432");

    if (PQstatus(conn) != CONNECTION_OK)
    {
        std::cout << "Database connection failed  !!!" << PQerrorMessage(conn) << std::endl;
        PQfinish(conn);
        exit(1);
    }

    int ver = PQserverVersion((const PGconn*)conn);
    int verMajor = int(ver / 10000);
    int verMinor = (ver % 10000);


    //Adaugare obiecte in db:

    //// Append the SQL statment
    //std::string sSQL;
    //sSQL.append("INSERT INTO employee VALUES ('Popescu','Maria')");

    //// Execute with sql statement
    //PGresult* res = PQexec(conn, sSQL.c_str());

    //if (PQresultStatus(res) != PGRES_COMMAND_OK)
    //{
    //    printf("Insert employee record failed");
    //    PQclear(res);
    //    PQfinish(conn);
    //}

    //printf("Insert employee record - OK\n");

    //// Clear result
    //PQclear(res);





}

PGconn* DatabaseConnection::GetConn()
{
    return this->conn;
}

void DatabaseConnection::CreateTables(PGconn* conn)
{

    // Execute with sql statement
    PGresult* res;


    res = PQexec(conn, "CREATE TABLE IF NOT EXISTS Users (username varchar(50) primary key, bio varchar(50), website varchar(50), birthday varchar(50), name varchar(50), location varchar(50));");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("Create Users table failed\n");
        PQclear(res);
        PQfinish(conn);
        return;
    }

    printf("Create Users table - OK\n");

    // Clear result
    PQclear(res);

    res = PQexec(conn, "CREATE TABLE IF NOT EXISTS Tweet (ID serial primary key, text varchar(141), date varchar(50), time varchar(50), ID_user varchar(50), CONSTRAINT fk_users FOREIGN KEY(ID_user) REFERENCES users(username));");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("Create Tweet table failed\n");
        PQclear(res);
        PQfinish(conn);
        return;
    }

    printf("Create Tweet table - OK\n");

    PQclear(res);

    res = PQexec(conn, "CREATE TABLE IF NOT EXISTS Comment (ID serial primary key, text varchar(50), date varchar(50), time varchar(50), ID_tweet int, ID_user varchar(50), CONSTRAINT fk_users FOREIGN KEY(ID_user) REFERENCES users(username), CONSTRAINT fk_tweet FOREIGN KEY(ID_tweet) REFERENCES tweet(ID))");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("Create Comment table failed\n");
        PQclear(res);
        PQfinish(conn);
        return;
    }

    printf("Create Comment table - OK\n");

    PQclear(res);

    res = PQexec(conn, "CREATE TABLE IF NOT EXISTS Retweet (ID serial primary key, text varchar(50), date varchar(50), time varchar(50), ID_tweet int, ID_user varchar(50), CONSTRAINT fk_users FOREIGN KEY(ID_user) REFERENCES users(username), CONSTRAINT fk_tweet FOREIGN KEY(ID_tweet) REFERENCES tweet(ID))");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("Create Retweet table failed\n");
        PQclear(res);
        PQfinish(conn);
        return;
    }

    printf("Create Retweet table - OK\n");

    PQclear(res);

    res = PQexec(conn, "CREATE TABLE IF NOT EXISTS Friends (ID serial primary key, username1 varchar(50), username2 varchar(50), CONSTRAINT fk_user1 FOREIGN KEY(username1) REFERENCES users(username), CONSTRAINT fk_user2 FOREIGN KEY(username2) REFERENCES users(username))");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("Create Retweet table failed\n");
        PQclear(res);
        PQfinish(conn);
        return;
    }

    printf("Create Friends table - OK\n");

    PQclear(res);

    res = PQexec(conn, "CREATE TABLE IF NOT EXISTS Likes (ID serial primary key, username varchar(50), ID_tweet int, CONSTRAINT fk_user FOREIGN KEY(username) REFERENCES users(username), CONSTRAINT fk_tweet FOREIGN KEY(ID_tweet) REFERENCES tweet(ID));");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("Create Likes table failed\n");
        PQclear(res);
        PQfinish(conn);
        return;
    }

    printf("Create Likes table - OK\n");

    // Clear result
    PQclear(res);

    PQfinish(conn);
    
}