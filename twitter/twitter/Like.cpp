#include "Like.h"

Like::Like(int ID, User user):
	m_ID(ID),
	m_user(user)
{
}


int Like::GetID() const
{
	return this->m_ID;
}

User Like::GetUser() const
{
	return this->m_user;
}

void Like::SetID( int ID)
{
	this->m_ID=ID;
}

void Like::SetUser(const User& user)
{
	this->m_user = user;
}

int Like::operator<(const Like & like) const
{
	return !this->m_ID < !like.GetID();
}

bool Like::operator==(const Like & like) const
{
	if (this->m_ID == like.GetID())
		return true;
	return false;
}

