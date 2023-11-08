#include "Design.h"

Design::Design(sf::RenderTarget* target)
{
	this->m_font.loadFromFile("arial.ttf");

	this->m_color.Blue = sf::Color(0xDA1F2FF);
	this->m_color.Black = sf::Color(0x14171AFF);
	this->m_color.DarkGray = sf::Color(0x657786FF);
	this->m_color.LightGray = sf::Color(0xAAB8C2FF);
	this->m_color.ExtraLightGray = sf::Color(0xE1E8EDFF);
	this->m_color.ExtraExtraLightGray = sf::Color(0xF5F8FAFF);

	//WIP
	int aux = 0;
	Button but9(target->getSize().x / 16, target->getSize().y/30 + (target->getSize().y / 20 + target->getSize().y / 35) * aux,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "(LogoPasare)",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue);
	this->m_button.LogoButton = but9;
	aux++;

	Button but(target->getSize().x/16,target->getSize().y/30 + (target->getSize().y / 20 + target->getSize().y / 35) *aux,
		target->getSize().x/8,target->getSize().y/20, &this->m_font, "Home",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue);
	this->m_button.Home = but;
	aux++;

	Button but1(target->getSize().x / 16, target->getSize().y / 30+ (target->getSize().y / 20 + target->getSize().y / 35) *aux,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "Explore",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue);
	this->m_button.Explore = but1;
	aux++;

	Button but8(target->getSize().x / 16, target->getSize().y / 30 + (target->getSize().y / 20 + target->getSize().y / 35) * aux,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "Notifications",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue);
	this->m_button.Notifications = but8;
	aux++;

	Button but2(target->getSize().x / 16, target->getSize().y / 30+ (target->getSize().y / 20 + target->getSize().y / 35) *aux,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "Messages",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue);
	this->m_button.Messages = but2;
	aux++;

	Button but3(target->getSize().x / 16, target->getSize().y / 30 + (target->getSize().y / 20 + target->getSize().y / 35) * aux,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "Bookmarks",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue);
	this->m_button.Bookmarks = but3;
	aux++;

	Button but4(target->getSize().x / 16, target->getSize().y / 30 + (target->getSize().y / 20 + target->getSize().y / 35) * aux,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "Lists",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue);
	this->m_button.Lists = but4;
	aux++;

	Button but5(target->getSize().x / 16, target->getSize().y / 30 + (target->getSize().y / 20 + target->getSize().y / 35) * aux,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "Profile",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue);
	this->m_button.Profile = but5;
	aux++;

	Button but6(target->getSize().x / 16, target->getSize().y / 30 + (target->getSize().y / 20 + target->getSize().y / 35) * aux,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "More",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue);
	this->m_button.More = but6;
	aux++;

	Button but7(target->getSize().x / 16, target->getSize().y / 30 + (target->getSize().y / 20 + target->getSize().y / 35) * aux + target->getSize().y / 35,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "Tweet",
		this->m_color.Blue, this->m_color.DarkGray,
		this->m_color.ExtraExtraLightGray);
	this->m_button.Tweet = but7;
	//  \WIP


	this->m_lineLeft = sf::VertexArray(sf::Lines, 2);
	this->m_lineRight = sf::VertexArray(sf::Lines, 2);
	this->m_lineTitle = sf::VertexArray(sf::Lines, 2);
	this->m_lineLeft[0] = sf::Vector2f(target->getSize().x / 4, 0), m_color.DarkGray;
	this->m_lineLeft[1] = sf::Vector2f(target->getSize().x / 4, target->getSize().y), m_color.DarkGray;
	this->m_lineRight[0] = sf::Vector2f(target->getSize().x / 4 * 3, 0), m_color.DarkGray;
	this->m_lineRight[1] = sf::Vector2f(target->getSize().x / 4 * 3, target->getSize().y), m_color.DarkGray;
	this->m_lineTitle[0] = sf::Vector2f(target->getSize().x / 4, target->getSize().y / 10), m_color.DarkGray;
	this->m_lineTitle[1] = sf::Vector2f(target->getSize().x / 4 * 3,target->getSize().y/10), m_color.DarkGray;
}
void Design::DrawTo(sf::RenderWindow *target)
{
	/*
	target->draw(m_lineLeft);
	target->draw(m_lineRight);
	target->draw(m_lineTitle);
	this->m_button.LogoButton.DrawTo(target);
	this->m_button.Home.DrawTo(target);
	this->m_button.Explore.DrawTo(target);
	this->m_button.Notifications.DrawTo(target);
	this->m_button.Messages.DrawTo(target);
	this->m_button.Bookmarks.DrawTo(target);
	this->m_button.Lists.DrawTo(target);
	this->m_button.Profile.DrawTo(target);
	this->m_button.More.DrawTo(target);
	this->m_button.Tweet.DrawTo(target);
	*/

}

void Design::SetColor(const sf::Color& color)
{
	m_lineLeft[0].color = color;
	m_lineLeft[1].color = color;
	m_lineRight[0].color = color;
	m_lineRight[1].color = color;
	m_lineTitle[0].color = color;
	m_lineTitle[1].color = color;
}

Design::~Design()
{
}

Design::Design()
{
}

