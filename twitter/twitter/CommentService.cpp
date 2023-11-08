#include "CommentService.h"

void CommentService::AddComment(const std::string& text, const std::string& date, const std::string& time, const std::string& id_user, int id_tweet)
{
    DatabaseConnection db;

    std::string id_tweetS = std::to_string(id_tweet);

    std::string sSQL;
    sSQL.append("INSERT INTO Comment(text,date,time,id_user,id_tweet) VALUES ('");
    sSQL.append(text);
    sSQL.append("', '");
    sSQL.append(date);
    sSQL.append("', '");
    sSQL.append(id_user);
    sSQL.append("', '");
    sSQL.append(id_tweetS);
    sSQL.append("')");

    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nInsert Comment record failed\n");
        PQclear(res);
        PQfinish(db.GetConn());
        return;
    }

    printf("\nInsert Comment record - OK\n");

    PQclear(res);

    PQfinish(db.GetConn());
}

void CommentService::DeleteComment(int id_comment)
{
    DatabaseConnection db;

    std::string id_commentS = std::to_string(id_comment);

    std::string sSQL;
    sSQL.append("DELETE FROM Comment WHERE id=");
    sSQL.append(id_commentS);

    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("Delete Comment record failed.");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nDelete Comment record - OK\n");

    PQclear(res);
}

void CommentService::EditComment(int id_comment, const std::string& newText)
{
    DatabaseConnection db;

    std::string id_commentS = std::to_string(id_comment);

    std::string sSQL;
    sSQL.append("UPDATE Retweet SET text='");
    sSQL.append(newText);
    sSQL.append("' WHERE ID='");
    sSQL.append(id_commentS);
    sSQL.append("';");

    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nUpdate text record failed.\n");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nUpdate text record - OK\n");

    PQclear(res);
}

int CommentService::GetId(const std::string& text, const std::string& date, const std::string& time, const std::string& id_user, int id_tweet)
{
    DatabaseConnection db;
    int id = -1;
    int nFields;

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    std::string id_tweetS = std::to_string(id_tweet);

    std::string sSQL;
    sSQL.append("DECLARE commrec CURSOR FOR select id from Comment where text='");
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

    res = PQexec(db.GetConn(), sSQL.c_str());
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in commrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    nFields = PQnfields(res);

    for (int i = 0; i < PQntuples(res); i++)
    {
        for (int j = 0; j < nFields; j++)
        {
            id = atoi(PQgetvalue(res, i, j));
        }
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "CLOSE commrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    PQfinish(db.GetConn());

    return id;
}

std::string CommentService::GetText(int id_comment)const
{
    DatabaseConnection db;
    std::string text;
    std::string id_commentS = std::to_string(id_comment);

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE commrec CURSOR FOR select * from comment");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in commrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == id_commentS)
        {
            text = PQgetvalue(res, i, 1);
            break;
        }
    }

    PQclear(res);


    res = PQexec(db.GetConn(), "CLOSE commrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    return text;
}

std::string CommentService::GetDate(int id_comment)const
{
    DatabaseConnection db;
    std::string date;
    std::string id_commentS = std::to_string(id_comment);

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE commrec CURSOR FOR select * from comment");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in commrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == id_commentS)
        {
            date = PQgetvalue(res, i, 2);
            break;
        }
    }

    PQclear(res);


    res = PQexec(db.GetConn(), "CLOSE commrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    return date;
}

std::string CommentService::GetTime(int id_comment)const
{
    DatabaseConnection db;
    std::string time;
    std::string id_commentS = std::to_string(id_comment);

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE commrec CURSOR FOR select * from comment");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in commrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == id_commentS)
        {
            time = PQgetvalue(res, i, 3);
            break;
        }
    }

    PQclear(res);


    res = PQexec(db.GetConn(), "CLOSE commrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    return time;
}

std::string CommentService::GetUser(int id_comment)const
{
    DatabaseConnection db;
    std::string username;
    std::string id_commentS = std::to_string(id_comment);

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE commrec CURSOR FOR select * from comment");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in commrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == id_commentS)
        {
            username = PQgetvalue(res, i, 5);
            break;
        }
    }

    PQclear(res);


    res = PQexec(db.GetConn(), "CLOSE commrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    return username;
}

int CommentService::GetTweet(int id_comment)const
{
    DatabaseConnection db;
    std::string id_tweetS;
    int id_tweet;
    std::string id_commentS = std::to_string(id_comment);

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE commrec CURSOR FOR select * from comment");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in commrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == id_commentS)
        {
            id_tweetS = PQgetvalue(res, i, 4);
            break;
        }
    }

    PQclear(res);


    res = PQexec(db.GetConn(), "CLOSE commrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    id_tweet = stoi(id_tweetS);
    return id_tweet;
}

void CommentService::SetText( int id_comment, const std::string& newText)
{
    DatabaseConnection db;
    std::string id_commentS = std::to_string(id_comment);

    std::string sSQL;
    sSQL.append("UPDATE comment SET text='");
    sSQL.append(newText);
    sSQL.append("' WHERE username='");
    sSQL.append(id_commentS);
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

void CommentService::SetDate(int id_comment, const std::string& newDate)
{
    DatabaseConnection db;
    std::string id_commentS = std::to_string(id_comment);

    std::string sSQL;
    sSQL.append("UPDATE comment SET date='");
    sSQL.append(newDate);
    sSQL.append("' WHERE username='");
    sSQL.append(id_commentS);
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

void CommentService::SetTime(int id_comment, const std::string& newTime)
{
    DatabaseConnection db;
    std::string id_commentS = std::to_string(id_comment);

    std::string sSQL;
    sSQL.append("UPDATE comment SET time='");
    sSQL.append(newTime);
    sSQL.append("' WHERE username='");
    sSQL.append(id_commentS);
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

void CommentService::SetUser( int id_comment, const std::string& newUser)
{
    DatabaseConnection db;
    std::string id_commentS = std::to_string(id_comment);

    std::string sSQL;
    sSQL.append("UPDATE comment SET id_user='");
    sSQL.append(newUser);
    sSQL.append("' WHERE username='");
    sSQL.append(id_commentS);
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

void CommentService::SetTweet( int id_comment, int newTweet)
{
    DatabaseConnection db;
    std::string id_commentS = std::to_string(id_comment);
    std::string id_tweetS = std::to_string(newTweet);

    std::string sSQL;
    sSQL.append("UPDATE comment SET id_tweet='");
    sSQL.append(id_tweetS);
    sSQL.append("' WHERE username='");
    sSQL.append(id_commentS);
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
