#pragma once

#include "libpq-fe.h"

class DatabaseConnection
{
public:
    DatabaseConnection();

    PGconn* GetConn();
    //Create Location table 
    void CreateTables(PGconn* conn);

private:
    const char* conninfo;
    PGconn* conn;
};