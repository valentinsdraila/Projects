#include "LikeService.h"

void LikeService::AddLike(std::string username, int id_tweet)
{
    // Adaugare obiecte in db:
    DatabaseConnection db;

    std::string id_tweetS = std::to_string(id_tweet);

    // Append the SQL statment
    std::string sSQL;
    sSQL.append("INSERT INTO Likes (username,id_tweet) VALUES ('");
    sSQL.append(username);
    sSQL.append("', '");
    sSQL.append(id_tweetS);
    sSQL.append("');");

    // Execute with sql statement
    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nInsert Likes record failed\n");
        PQclear(res);
        PQfinish(db.GetConn());
        return;
    }

    printf("\nInsert Likes record - OK\n");

    // Clear result
    PQclear(res);

    PQfinish(db.GetConn());
}

void LikeService::DeleteLike(int id_like)
{
    DatabaseConnection db;

    std::string id_likeS = std::to_string(id_like);

    // Append the SQL statment
    std::string sSQL;
    sSQL.append("DELETE FROM Likes WHERE id=");
    sSQL.append(id_likeS);

    // Execute with sql statement
    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("Delete Likes record failed.");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nDelete Likes record - OK\n");

    // Clear result
    PQclear(res);
}

int LikeService::GetId(std::string id_user, int id_tweet)
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
    sSQL.append("DECLARE likerec CURSOR FOR select id from Likes where username='");
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

    res = PQexec(db.GetConn(), "FETCH ALL in likerec");

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
    res = PQexec(db.GetConn(), "CLOSE likerec");
    PQclear(res);

    // End the transaction
    res = PQexec(db.GetConn(), "END");

    // Clear result
    PQclear(res);

    PQfinish(db.GetConn());

    return id;
}
