#include "UserService.h"

void UserService::AddUser(const std::string& username,const  std::string& bio,const  std::string& website,const std::string& birthday,const std::string& name,const std::string& location)
{
    DatabaseConnection db;

    std::string sSQL;
    sSQL.append("INSERT INTO users(username, bio, website, birthday, name, location) VALUES ('");
    sSQL.append(username);
    sSQL.append("', '");
    sSQL.append(bio);
    sSQL.append("', '");
    sSQL.append(website);
    sSQL.append("', '");
    sSQL.append(birthday);
    sSQL.append("', '");
    sSQL.append(name);
    sSQL.append("', '");
    sSQL.append(location);
    sSQL.append("')");

    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nInsert User record failed\n");
        PQclear(res);
        PQfinish(db.GetConn());
        return;
    }

    printf("\nInsert User record - OK\n");

    PQclear(res);

    PQfinish(db.GetConn());
}

void UserService::RemoveUser(const std::string& username)
{
    DatabaseConnection db;

    std::string sSQL;
    sSQL.append("DELETE FROM user WHERE username='");
    sSQL.append(username);
    sSQL.append("'");

    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nDelete user record failed.\n");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nDelete user record - OK\n");

    PQclear(res);
}

bool UserService::CheckUser(const std::string& username)
{
    DatabaseConnection db;

    int nFields;

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    std::string sSQL;
    sSQL.append("DECLARE userrec CURSOR FOR SELECT username from users where username = '");
    sSQL.append(username);
    sSQL.append("'");

    res = PQexec(db.GetConn(), sSQL.c_str());
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

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
                PQclear(res);

                res = PQexec(db.GetConn(), "CLOSE userrec");
                PQclear(res);

                res = PQexec(db.GetConn(), "END");

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

void UserService::SetBio(const std::string& username, const std::string& bio)
{
    DatabaseConnection db;

    std::string sSQL;
    sSQL.append("UPDATE users SET bio='");
    sSQL.append(bio);
    sSQL.append("' WHERE username='");
    sSQL.append(username);
    sSQL.append("';");

    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nUpdate bio record failed.\n");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nUpdate bio record - OK\n");

    PQclear(res);
}

void UserService::SetWebsite(const std::string& username, const std::string& website)
{
    DatabaseConnection db;

    std::string sSQL;
    sSQL.append("UPDATE users SET website='");
    sSQL.append(website);
    sSQL.append("' WHERE username='");
    sSQL.append(username);
    sSQL.append("';");

    PGresult* res = PQexec(db.GetConn(), sSQL.c_str());

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("\nUpdate website record failed.\n");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    printf("\nUpdate website record - OK\n");

    PQclear(res);
}

void UserService::SetLocation(const std::string& username, const std::string& location)
{
    DatabaseConnection db;

    std::string sSQL;
    sSQL.append("UPDATE users SET location='");
    sSQL.append(location);
    sSQL.append("' WHERE username='");
    sSQL.append(username);
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

std::string UserService::GetBirthday(const std::string& username)const
{
    DatabaseConnection db;
    std::string birthday;

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE userrec CURSOR FOR select * from users");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in userrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == username)
        {
            birthday = PQgetvalue(res, i, 3);
            break;
        }
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "CLOSE userrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    return birthday;
}

std::string UserService::GetName(const std::string& username)const
{
    DatabaseConnection db;
    std::string name;

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE userrec CURSOR FOR select * from users");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in userrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == username)
        {
            name = PQgetvalue(res, i, 4);
            break;
        }
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "CLOSE userrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    // Clear result
    PQclear(res);

    return name;
}

std::string UserService::GetBio(const std::string& username)const
{
    DatabaseConnection db;
    std::string bio;

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE userrec CURSOR FOR select * from users");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in userrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == username)
        {
            bio = PQgetvalue(res, i, 1);
            break;
        }
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "CLOSE userrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    return bio;
}

std::string UserService::GetWebsite(const std::string& username)const
{
    DatabaseConnection db;
    std::string website;

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE userrec CURSOR FOR select * from users");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in userrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == username)
        {
            website = PQgetvalue(res, i, 2);
            break;
        }
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "CLOSE userrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    return website;
}

std::string UserService::GetLocation(const std::string& username)const
{
    DatabaseConnection db;
    std::string location;

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE userrec CURSOR FOR select * from users");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in userrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == username)
        {
            location = PQgetvalue(res, i, 5);
            break;
        }
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "CLOSE userrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    return location;
}

std::vector<std::string> UserService::GetUser(const std::string& username)const
{
    std::vector<std::string> userDetails;
    DatabaseConnection db;

    PGresult* res = PQexec(db.GetConn(), "BEGIN");

    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("BEGIN command failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "DECLARE userrec CURSOR FOR select * from users");
    if (PQresultStatus(res) != PGRES_COMMAND_OK)
    {
        printf("DECLARE CURSOR failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "FETCH ALL in userrec");

    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        printf("FETCH ALL failed");
        PQclear(res);
        PQfinish(db.GetConn());
    }

    for (int i = 0; i < PQntuples(res); i++)
    {
        if (PQgetvalue(res, i, 0) == username)
        {
            userDetails.push_back( PQgetvalue(res, i, 1));
            userDetails.push_back( PQgetvalue(res, i, 2));
            userDetails.push_back( PQgetvalue(res, i, 3));
            userDetails.push_back( PQgetvalue(res, i, 4));
            userDetails.push_back( PQgetvalue(res, i, 5));
            break;
        }
    }

    PQclear(res);

    res = PQexec(db.GetConn(), "CLOSE userrec");
    PQclear(res);

    res = PQexec(db.GetConn(), "END");

    PQclear(res);

    return userDetails;
}
