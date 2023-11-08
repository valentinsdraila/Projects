#include "RetweetService.h"

void RetweetService::AddRetweet(const std::string& text, const std::string& date, const std::string& time, const std::string& id_user, int id_tweet)
{
    // Adaugare obiecte in db:
    DatabaseConnection db;

    std::string id_tweetS = std::to_string(id_tweet);

    // Append the SQL statment
    std::string sSQL;
    sSQL.append("INSERT INTO Retweet(text,date,time,id_user,id_tweet) VALUES ('");
    sSQL.append(text);
    sSQL.append("', '");
    sSQL.append(date);
    sSQL.append("', '");
    sSQL.append(id_user);
    sSQL.append("', '");
    sSQL.append(id_tweetS);
    sSQL.append("')");

    // Execute with sql statement
    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nInsert Retweet record failed\n");
        PQclear(res);
        PQfinish(db.GetConn());
        return;
    }

    printf("\nInsert Retweet record - OK\n");

    // Clear result
    PQclear(res);

    PQfinish(db.GetConn());
}

void RetweetService::DeleteRetweet(int id_retweet)
{
    DatabaseConnection db;

    std::string id_retweetS = std::to_string(id_retweet);

    // Append the SQL statment
    std::string sSQL;
    sSQL.append("DELETE FROM Retweet WHERE id=");
    sSQL.append(id_retweetS);

    // Execute with sql statement
    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("Delete Retweet record failed.");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nDelete Retweet record - OK\n");

    // Clear result
    PQclear(res);
}

void RetweetService::EditRetweet(int id_retweet, const std::string& newText)
{
    DatabaseConnection db;

    std::string id_retweetS = std::to_string(id_retweet);

    // Append the SQL statment
    std::string sSQL;
    sSQL.append("UPDATE Retweet SET text='");
    sSQL.append(newText);
    sSQL.append("' WHERE ID='");
    sSQL.append(id_retweetS);
    sSQL.append("';");

    // Execute with sql statement
    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nUpdate text record failed.\n");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nUpdate text record - OK\n");

    // Clear result
    PQclear(res);
}

int RetweetService::GetId(const std::string& text, const std::string& date, const std::string& time, const std::string& id_user, int id_tweet)
{
    DatabaseConnection db;
    int id = -1;
    // Will hold the number of field in employee table
    int nFields;

    // Start a transaction block
    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    // Clear result
    PQclear(res);

    std::string id_tweetS = std::to_string(id_tweet);

    // Append the SQL statment
    std::string sSQL;
    sSQL.append("DECLARE retweetrec CURSOR FOR select id from Retweet where text='");
    sSQL.append(text);
    sSQL.append("' AND date='");
    sSQL.append(date);
    sSQL.append("' AND time='");
    sSQL.append(time);
    sSQL.append("' AND id_user='");
    sSQL.append(id_user);
    sSQL.append("' AND id_tweet='");
    sSQL.append(id_tweetS);
    sSQL.append("'");

    // Fetch rows from Tweet table  
    res = PQexec(db.GetConn(), sSQL.c_str());
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    // Clear result
    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in retweetrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    // Get the field name
    nFields = PQnfields(res);

    // Next, print out the Tweet record for each row
    for (int i = 0; i < PQntuples(res); i++)
    {
        for (int j = 0; j < nFields; j++)
        {
            id = atoi(PQgetvalue(res, i, j));
        }
    }

    PQclear(res);

    // Close the retweetrec
    res = PQexec(db.GetConn(), "CLOSE retweetrec");
    PQclear(res);

    // End the transaction
    res = PQexec(db.GetConn(), "END");

    // Clear result
    PQclear(res);

    PQfinish(db.GetConn());

    return id;
}

std::string RetweetService::GetText(int id_retweet)const
{
    DatabaseConnection db;
    std::string text;
    std::string id_retweetS = std::to_string(id_retweet);

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE retrec CURSOR FOR select * from retweet");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in retrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == id_retweetS)
        {
            text = PQgetvalue(res, i, 1);
            break;
        }
    }

    PQclear(res);


    res = PQexec(db.GetConn(), "CLOSE retrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    return text;
}

std::string RetweetService::GetDate(int id_retweet)const
{
    DatabaseConnection db;
    std::string date;
    std::string id_retweetS = std::to_string(id_retweet);

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE retrec CURSOR FOR select * from retweet");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in retrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == id_retweetS)
        {
            date = PQgetvalue(res, i, 1);
            break;
        }
    }

    PQclear(res);


    res = PQexec(db.GetConn(), "CLOSE retrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    return date;
}

std::string RetweetService::GetTime(int id_retweet)const
{
    DatabaseConnection db;
    std::string time;
    std::string id_retweetS = std::to_string(id_retweet);

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE retrec CURSOR FOR select * from retweet");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in retrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == id_retweetS)
        {
            time = PQgetvalue(res, i, 1);
            break;
        }
    }

    PQclear(res);


    res = PQexec(db.GetConn(), "CLOSE retrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    return time;
}

std::string RetweetService::GetUser(int id_retweet)const
{
    DatabaseConnection db;
    std::string username;
    std::string id_retweetS = std::to_string(id_retweet);

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE retrec CURSOR FOR select * from retweet");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in retrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == id_retweetS)
        {
            username = PQgetvalue(res, i, 1);
            break;
        }
    }

    PQclear(res);


    res = PQexec(db.GetConn(), "CLOSE retrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    return username;
}

int RetweetService::GetTweet(int id_retweet)const
{
    DatabaseConnection db;
    std::string id_tweetS;
    int id_tweet;
    std::string id_retweetS = std::to_string(id_retweet);

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE retrec CURSOR FOR select * from retweet");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in retrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == id_retweetS)
        {
            id_tweetS = PQgetvalue(res, i, 1);
            break;
        }
    }

    PQclear(res);


    res = PQexec(db.GetConn(), "CLOSE retrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    id_tweet = stoi(id_tweetS);
    return id_tweet;
}

void RetweetService::SetText(int id_retweet, const std::string& newText)
{
    DatabaseConnection db;
    std::string id_retweetS = std::to_string(id_retweet);

    std::string sSQL;
    sSQL.append("UPDATE comment SET text='");
    sSQL.append(newText);
    sSQL.append("' WHERE username='");
    sSQL.append(id_retweetS);
    sSQL.append("';");

    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nUpdate location record failed.\n");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nUpdate location record - OK\n");

    PQclear(res);
}

void RetweetService::SetDate(int id_retweet, const std::string& newDate)
{
    DatabaseConnection db;
    std::string id_retweetS = std::to_string(id_retweet);

    std::string sSQL;
    sSQL.append("UPDATE comment SET date='");
    sSQL.append(newDate);
    sSQL.append("' WHERE username='");
    sSQL.append(id_retweetS);
    sSQL.append("';");

    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nUpdate location record failed.\n");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nUpdate location record - OK\n");

    PQclear(res);
}

void RetweetService::SetTime(int id_retweet, const std::string& newTime)
{
    DatabaseConnection db;
    std::string id_retweetS = std::to_string(id_retweet);

    std::string sSQL;
    sSQL.append("UPDATE comment SET time='");
    sSQL.append(newTime);
    sSQL.append("' WHERE username='");
    sSQL.append(id_retweetS);
    sSQL.append("';");

    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nUpdate location record failed.\n");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nUpdate location record - OK\n");

    PQclear(res);
}

void RetweetService::SetUser(int id_retweet, const std::string& newUser)
{
    DatabaseConnection db;
    std::string id_retweetS = std::to_string(id_retweet);

    std::string sSQL;
    sSQL.append("UPDATE comment SET id_user='");
    sSQL.append(newUser);
    sSQL.append("' WHERE username='");
    sSQL.append(id_retweetS);
    sSQL.append("';");

    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nUpdate location record failed.\n");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nUpdate location record - OK\n");

    PQclear(res);
}

void RetweetService::SetTweet(int id_retweet, const int newTweet)
{
    DatabaseConnection db;
    std::string id_retweetS = std::to_string(id_retweet);
    std::string id_tweetS = std::to_string(newTweet);

    std::string sSQL;
    sSQL.append("UPDATE comment SET id_tweet='");
    sSQL.append(id_tweetS);
    sSQL.append("' WHERE username='");
    sSQL.append(id_retweetS);
    sSQL.append("';");

    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nUpdate location record failed.\n");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nUpdate location record - OK\n");

    PQclear(res);
}
