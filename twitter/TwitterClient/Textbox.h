#pragma once
#include <iostream> 
#include <SFML/Graphics.hpp>
#include <sstream>
#define DELETE_KEY 8
#define ENTER_KEY 13
#define ESCAPE_KEY 27
class Textbox
{
public:
	Textbox();
	Textbox(int size, sf::Color color, bool sel = false);
	void SetFont(const sf::Font& font);
	void SetPosition(sf::Vector2f pos);
	void SetLimit(const bool& ToF);
	void SetLimit(const bool& ToF,const int& limit);
	void SetSelected(const bool& sel);
	void SetText(const std::string& text);
	std::string GetText()const ;
	void DrawTo(sf::RenderTarget* target);
	void Type(sf::Event input);
	Textbox &operator=(const Textbox& t1);
	void NewLine();
	bool IsSelected();
private:
	sf::Text m_textbox;
	std::ostringstream m_text;
	bool m_isSelected = false;
	bool m_hasLimit = false;
	int m_limit;
	void InputLogic(int charTyped);
	void DeleteLastChar();
	
};

