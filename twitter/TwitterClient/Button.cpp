#include "Button.h"

Button::Button()
{
}

Button::Button(float x, float y, float width, float height,
	sf::Font* font, std::string text, sf::Color idleColor,
	sf::Color hoverColor, sf::Color activeColor,sf::Color outlineColor)
{
	this->buttonState = buttonIdle;

	this->m_shape.setPosition(sf::Vector2f(x, y));
	this->m_shape.setSize(sf::Vector2f(width, height));
	this->m_font = font;
	this->m_text.setFont(*this->m_font);
	this->m_text.setString(text);
	this->m_text.setFillColor(sf::Color::White);
	this->m_text.setCharacterSize(15);
	this->m_text.setPosition(
		this->m_shape.getPosition().x +(this->m_shape.getGlobalBounds().width/2.f) - this->m_text.getGlobalBounds().width/2.f,
		this->m_shape.getPosition().y +(this->m_shape.getGlobalBounds().height/2.f) - this->m_text.getGlobalBounds().height / 2.f
	);

	this->m_idleColor = idleColor;
	this->m_hoverColor = hoverColor;
	this->m_activeColor = activeColor;
	this->m_shape.setFillColor(this->m_idleColor);
	this->m_shape.setOutlineThickness(1);
	this->m_shape.setOutlineColor(outlineColor);
}

Button::~Button()
{
}

void Button::setCharSize(const float& charSize)
{
	this->m_text.setCharacterSize(charSize);
}

void Button::setTextStyle(const sf::Text::Style style)
{
	this->m_text.setStyle(style);
}

void Button::setPosition(sf::Vector2f pos)
{
	this->m_text.setPosition(pos.x, pos.y);
}

void Button::SetText(const std::string& text)
{
	this->m_text.setString(text);
}

short unsigned int Button::getButtonState()
{
	return this->buttonState;
}

void Button::changeButtonStateIdle()
{
	this->buttonState = 0;
}

const bool Button::isPressed() const
{
	if (this->buttonState == buttonActive)
		return true;
	return false;
}

	/*Update bool pentru hover si pressed*/
void Button::update(const sf::Vector2f mousePos)
{
	//Idle
	this->buttonState = buttonIdle;
	//Hover
	if (this->m_shape.getGlobalBounds().contains(mousePos))
	{
		this->buttonState = buttonHover;
		//Pressed
		if (sf::Mouse::isButtonPressed(sf::Mouse::Left))
		{
			this->buttonState = buttonActive;
			
		}
	}

	switch (this->buttonState)
	{
	case buttonIdle:
		this->m_shape.setFillColor(this->m_idleColor);
		break;

	case buttonHover:
		this->m_shape.setFillColor(this->m_hoverColor);
		break;

	case buttonActive:
		this->m_shape.setFillColor(this->m_activeColor);
		break;

	default:
		this->m_shape.setFillColor(sf::Color::Magenta);
		break;
	}
}

void Button::render(sf::RenderTarget* target)
{
	target->draw(this->m_shape);
	target->draw(this->m_text);
}

