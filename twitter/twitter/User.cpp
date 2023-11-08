#include "User.h"

// pentru inregistrare user
User::User(const std::string& username,const std::string& bio,const std::string& website,const std::string& birthday,const std::string& name,const std::string& location) :
	m_username(username),
	m_name(name),
	m_birthday(birthday),
	m_bio(bio),
	m_website(website),
	m_location(location)
{
	UserService userS;
	userS.AddUser(username, bio, website, birthday, name, location);
}

// pentru logare
User::User(const std::string& username)
{
	UserService userS;

	m_username = username;
	m_name = userS.GetName(username);
	m_birthday = userS.GetBirthday(username);
	m_bio = userS.GetBio(username);
	m_website = userS.GetWebsite(username);
	m_location = userS.GetLocation(username);
}

User::User(const std::string& username, bool direct)
{
	UserService userS;
	std::vector<std::string> userDetails = userS.GetUser(username);
	m_username = username;
	m_name = userDetails[3];
	m_birthday = userDetails[2];
	m_bio = userDetails[0];
	m_website = userDetails[1];
	m_location = userDetails[4];

}

User::User(const User& user)
{
	this->m_bio = user.GetBio();
	this->m_name = user.GetName();
	this->m_username = user.GetUsername();
	this->m_website = user.GetWebsite();
}

std::string User::GetUsername() const
{
	return m_username;
}

std::string User::GetName()const
{
	return m_name;
}

std::string User::GetBirthday()const
{
	return m_birthday;
}

std::string User::GetBio()const
{
	return m_bio;
}

std::string User::GetWebsite()const
{
	return m_website;
}

std::string User::GetLocation() const
{
	return this->m_location;
}

void User::SetUsername(const std::string& username)
{
	this->m_username = username;
}

void User::SetName(const std::string& name)
{
	this->m_name = name;
}

void User::SetBio(const std::string& bio)
{
	this->m_bio = bio;
}

void User::SetWebsite(const std::string& website)
{
	this->m_website = website;
}

void User::SetBirthday(const std::string& birthday)
{
	this->m_birthday = birthday;
}

void User::SetLocation(const std::string& location)
{
	this->m_location = location;
}

void User::CreateFeedPosts()
{
	///Iterate through followers and insert every post into priorityqueue
}

std::string User::GetPriorityPost()
{
	std::string post = m_posts.GetMaxElement();
	this->m_posts.ExtractMax();

	return post;
}

bool User::operator==(const User& user)
{
	if (this->m_bio != user.GetBio())
		return false;
	if (this->m_name != user.GetName())
		return false;
	if (this->m_username != user.GetUsername())
		return false;
	if (this->m_website != user.GetWebsite())
		return false;
	return true;
}
