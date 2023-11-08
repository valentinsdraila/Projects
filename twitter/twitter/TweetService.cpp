#include "TweetService.h"
#include <iostream>
#include <sstream>
#include "TweetService.h"
void TweetService::AddTweet(const std::string& text, const std::string& date, const std::string& time, const std::string& id_user)
{
    // Adaugare obiecte in db:
    DatabaseConnection db;

    // Append the SQL statment
    std::string sSQL;
    sSQL.append("INSERT INTO Tweet(text,date,time,id_user) VALUES ('");
    sSQL.append(text);
    sSQL.append("', '");
    sSQL.append(date);
    sSQL.append("', '");
    sSQL.append(time);
    sSQL.append("', '");
    sSQL.append(id_user);
    sSQL.append("')");

    // Execute with sql statement
    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nInsert Tweet record failed\n");
        PQclear(res);
        PQfinish(db.GetConn());
        return;
    }

    printf("\nInsert Tweet record - OK\n");

    // Clear result
    PQclear(res);

    PQfinish(db.GetConn());
}

void TweetService::DeleteTweet(int id)
{
    DatabaseConnection db;

    std::string id_tweet = std::to_string(id);

    // Append the SQL statment
    std::string sSQL;
    sSQL.append("DELETE FROM Tweet WHERE id=");
    sSQL.append(id_tweet);

    // Execute with sql statement
    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("Delete Tweet record failed.");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nDelete Tweet record - OK\n");

    // Clear result
    PQclear(res);
}

void TweetService::EditTweet(int id_tweet, const std::string& newText)
{
    DatabaseConnection db;

    std::string id = std::to_string(id_tweet);

    // Append the SQL statment
    std::string sSQL;
    sSQL.append("UPDATE Tweet SET text='");
    sSQL.append(newText);
    sSQL.append("' WHERE ID='");
    sSQL.append(id);
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

int TweetService::GetId(const std::string& text, const std::string& date, const std::string& time, const std::string& id_user)
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

    // Append the SQL statment
    std::string sSQL;
    sSQL.append("DECLARE tweetrec CURSOR FOR select id from Tweet where text='");
    sSQL.append(text);
    sSQL.append("' AND date='");
    sSQL.append(date);
    sSQL.append("' AND time='");
    sSQL.append(time);
    sSQL.append("' AND id_user='");
    sSQL.append(id_user);
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

    res = PQexec(db.GetConn(), "FETCH ALL in tweetrec");

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

    // Close the tweetrec
    res = PQexec(db.GetConn(), "CLOSE tweetrec");
    PQclear(res);

    // End the transaction
    res = PQexec(db.GetConn(), "END");

    // Clear result
    PQclear(res);

    PQfinish(db.GetConn());

    return id;
}

std::string TweetService::GetUserByTime(const std::string& time)
{
    DatabaseConnection db;
    // Will hold the number of field in employee table
    int nFields;

    std::string text = "";

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
    sSQL.append("DECLARE tweetrec CURSOR FOR select id_user from Tweet where time='");
    sSQL.append(time);
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

    res = PQexec(db.GetConn(), "FETCH ALL in tweetrec");

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
            text += PQgetvalue(res, i, j);
        }
    }

    PQclear(res);

    // Close the tweetrec
    res = PQexec(db.GetConn(), "CLOSE tweetrec");
    PQclear(res);

    // End the transaction
    res = PQexec(db.GetConn(), "END");

    // Clear result
    PQclear(res);

    PQfinish(db.GetConn());

    return text;
}

std::string TweetService::GetTweet(const std::string& time)
{
    DatabaseConnection db;
    // Will hold the number of field in employee table
    int nFields;

    std::string text = "";

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
    sSQL.append("DECLARE tweetrec CURSOR FOR select text from Tweet where time='");
    sSQL.append(time);
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

    res = PQexec(db.GetConn(), "FETCH ALL in tweetrec");

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
            text += PQgetvalue(res, i, j);
        }
    }

    PQclear(res);

    // Close the tweetrec
    res = PQexec(db.GetConn(), "CLOSE tweetrec");
    PQclear(res);

    // End the transaction
    res = PQexec(db.GetConn(), "END");

    // Clear result
    PQclear(res);

    PQfinish(db.GetConn());

    return text;
}

std::vector<std::string> TweetService::GetDate(const std::string& username)
{
    DatabaseConnection db;
    std::vector<std::string> date;
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
    sSQL.append("DECLARE tweetrec CURSOR FOR select date from Tweet where id_user='");
    sSQL.append(username);
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

    res = PQexec(db.GetConn(), "FETCH ALL in tweetrec");

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
            std::string data = "";
            date.push_back(PQgetvalue(res, i, j));
            for (int k = date[i].length() - 5; k < date[i].length() - 1; k++)
            {
                data += date[i][k];
            }
            data += ' ';
            for (int k = 0; k < 4; k++)
            {
                data += date[i][k];
            }
            for (int k = 4; k < 7; k++)
            {
                data += date[i][k];
            }
            date[i] = data;
        }
    }
    std::vector <std::string> tokens;
    std::string newDate;
    for (auto x : date)
    {
        newDate = "";

        // stringstream class check1
        std::stringstream check1(x);

        std::string intermediate;

        // Tokenizing w.r.t. space ' '
        int nr = 0;
        while (std::getline(check1, intermediate, ' '))
        {
            nr++;
            bool ok = false;
            if (intermediate == "Jan")
            {
                ok = true;
                newDate += "01";
            }
            if (intermediate == "Feb")
            {
                ok = true;
                newDate += "02";
            }
            if (intermediate == "Mar")
            {
                ok = true;
                newDate += "03";
            }
            if (intermediate == "Apr")
            {
                ok = true;
                newDate += "04";
            }
            if (intermediate == "May")
            {
                newDate += "05";
                ok = true;
            }
            if (intermediate == "Jun")
            {
                ok = true;
                newDate += "06";
            }
            if (intermediate == "Jul")
            {
                newDate += "07";
                ok = true;
            }
            if (intermediate == "Aug")
            {
                ok = true;
                newDate += "08";
            }
            if (intermediate == "Sep")
            {
                ok = true;
                newDate += "09";
            }
            if (intermediate == "Oct")
            {
                ok = true;
                newDate += "10";
            }
            if (intermediate == "Nov")
            {
                ok = true;
                newDate += "11";
            }
            if (intermediate == "Dec")
            {
                ok = true;
                newDate += "12";
            }
            if (ok == false)
            {
                if (nr == 3)
                {
                    if (x[9] == ' ')
                    {
                        newDate.append("0");
                        newDate += intermediate;
                    }
                    else
                        newDate += intermediate;
                }
                else
                    newDate += intermediate;
            }
        }
        tokens.push_back(newDate);
    }
    PQclear(res);

    // Close the tweetrec
    res = PQexec(db.GetConn(), "CLOSE tweetrec");
    PQclear(res);

    // End the transaction
    res = PQexec(db.GetConn(), "END");

    // Clear result
    PQclear(res);

    PQfinish(db.GetConn());

    return tokens;
}

std::vector<std::string> TweetService::GetTime(const std::string& username)
{
    DatabaseConnection db;
    std::vector<std::string> time;
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
    sSQL.append("DECLARE tweetrec CURSOR FOR select time from Tweet where id_user='");
   
    sSQL.append(username);
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

    res = PQexec(db.GetConn(), "FETCH ALL in tweetrec");

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
            time.push_back(PQgetvalue(res, i, j));
        }
    }

    PQclear(res);

    // Close the tweetrec
    res = PQexec(db.GetConn(), "CLOSE tweetrec");
    PQclear(res);

    // End the transaction
    res = PQexec(db.GetConn(), "END");

    // Clear result
    PQclear(res);

    PQfinish(db.GetConn());

    return time;
}
