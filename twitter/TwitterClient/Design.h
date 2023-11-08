#pragma once
#include <iostream>
#include <SFML/Graphics.hpp>
#include "Button.h"
#include "Textbox.h"
class Design
{
public:
	Design(sf::RenderTarget* target);
	void DrawTo(sf::RenderWindow* target);
	///<for alternate themes>
		void SetColor(const sf::Color& color);
	///<\for alternate themes>
		~Design();
		Design();
private:
	sf::Font m_font;

	// Twitter Colors
	struct TwitterColor {
		sf::Color Blue; //twitter logo,tweet button
		sf::Color Black;//background
		sf::Color DarkGray;//left stuff and button highlight
		sf::Color LightGray;//dunno
		sf::Color ExtraLightGray;// follow buttons and stuff and text??
		sf::Color ExtraExtraLightGray;//current button text highlight
	};

	TwitterColor m_color;
	//
	
public:
	//Twitter HomeButtons
	struct TwitterHomeButton
	{
		Button LogoButton;
		Button Home;
		Button Explore;
		Button Notifications;
		Button Messages;
		Button Bookmarks;
		Button Lists;
		Button Profile;
		Button More;
		Button Tweet;
	};
	TwitterHomeButton m_button;
	//
private:
	sf::VertexArray m_lineLeft;
	sf::VertexArray m_lineRight;
	sf::VertexArray m_lineTitle;
	//sf::VertexArray m_tweetLines;
};


