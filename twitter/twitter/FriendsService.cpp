#include "FriendsService.h"
#include <iostream>

void FriendsService::AddFriend(const std::string& username, const std::string& friendUsername)
{
    // Adaugare obiecte in db:
    DatabaseConnection db;

    // Append the SQL statment
    std::string sSQL,sSQL2;
    sSQL.append("INSERT INTO Friends(username1,username2) VALUES ('");
    sSQL.append(username);
    sSQL.append("', '");
    sSQL.append(friendUsername);
    sSQL.append("')");

    sSQL2.append("INSERT INTO Friends(username1,username2) VALUES ('");
    sSQL2.append(friendUsername);
    sSQL2.append("', '");
    sSQL2.append(username);
    sSQL2.append("')");

    // Execute with sql statement
    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nInsert Friends record failed\n");
        PQclear(res);
        PQfinish(db.GetConn());
        return;
    }

    printf("\nInsert Friends record - OK\n");

    // Clear result
    PQclear(res);

    PQfinish(db.GetConn());
}

void FriendsService::RemoveFriend(const std::string& username, const std::string& friendUsername)
{
    DatabaseConnection db;


    // Append the SQL statment
    std::string sSQL, sSQL2;
    sSQL.append("DELETE FROM Friends WHERE username1='");
    sSQL.append(username);
    sSQL.append("' AND username2='");
    sSQL.append(friendUsername);
    sSQL.append("'");

    sSQL2.append("DELETE FROM Friends WHERE username1='");
    sSQL2.append(friendUsername);
    sSQL2.append("' AND username2='");
    sSQL2.append(username);
    sSQL2.append("'");

    // Execute with sql statement
    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("Delete Friends record failed.");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nDelete Friends record - OK\n");

    // Clear result
    PQclear(res);
}

bool FriendsService::VerifyFriends(const std::string& username, const std::string& friendUsername)
{
    DatabaseConnection db;
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

    // Append the SQL statment
    std::string sSQL;
    sSQL.append("DECLARE userrec CURSOR FOR SELECT username1 from friends where username1 = '");
    sSQL.append(username);
    sSQL.append("'AND username2 = '");
    sSQL.append(friendUsername);
    sSQL.append("'");

    // Fetch rows from employee table  
    res = PQexec(db.GetConn(), sSQL.c_str());
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    // Clear result
    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in userrec");

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
            if (PQgetvalue(res, i, j) == username)
            {
                std::cout << PQgetvalue(res, i, j);

                PQclear(res);

                // Close the userrec
                res = PQexec(db.GetConn(), "CLOSE userrec");
                PQclear(res);

                // End the transaction
                res = PQexec(db.GetConn(), "END");

                // Clear result
                PQclear(res);

                PQfinish(db.GetConn());
                return true;
            }
        }
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "CLOSE userrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);
    PQfinish(db.GetConn());

    return false;

}

std::vector<std::string> FriendsService::GetFriendsList(const std::string& username)
{
    DatabaseConnection db;
    std::vector<std::string> friendsList;

    // Will hold the number of field in Friends table
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

    // Fetch rows from Friends table
    res = PQexec(db.GetConn(), "DECLARE frirec CURSOR FOR select * from friends");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    // Clear result
    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in frirec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }


    // Next, print out the employee record for each row
    for (int i = 0; i < PQntuples(res); i++)
    {

            if (PQgetvalue(res, i, 1)==username)
            {
                friendsList.push_back(PQgetvalue(res, i, 2));
            }
    }

    PQclear(res);

    // Close the emprec
    res = PQexec(db.GetConn(), "CLOSE frirec");
    PQclear(res);

    // End the transaction
    res = PQexec(db.GetConn(), "END");

    PQfinish(db.GetConn());

    return friendsList;
}
