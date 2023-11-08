#pragma once
#include <iostream>
#include <SFML/Graphics.hpp>

enum buttonStates {
	buttonIdle=0,
	buttonHover,
	buttonActive
};

class Button
{
private:
	sf::RectangleShape m_shape;
	sf::Font* m_font;
	sf::Text m_text;

	sf::Color m_idleColor;
	sf::Color m_hoverColor;
	sf::Color m_activeColor;

protected:
	short unsigned buttonState;

public:
	Button();
	Button(float x,float y,float width,float height,sf::Font* font,
		std::string text,sf::Color idleColor,sf::Color hoverColor,
		sf::Color activeColor,sf::Color outlineColor);
	~Button();
	
	void setCharSize(const float& charSize);
	void setTextStyle(const sf::Text::Style style);
	void setPosition(sf::Vector2f pos);
	void SetText(const std::string& text);

	short unsigned int getButtonState();
	void changeButtonStateIdle();

	const bool isPressed() const;

	void update(const sf::Vector2f mousePos);
	void render(sf::RenderTarget * target);

	
};