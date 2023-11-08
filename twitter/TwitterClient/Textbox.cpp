#include "Textbox.h"

Textbox::Textbox()
{
}

Textbox::Textbox(int size, sf::Color color, bool sel)
{
	m_textbox.setCharacterSize(size);
	m_textbox.setFillColor(color);
	m_isSelected = sel;
	if (sel)
	{
		m_textbox.setString("_");
	}
	else
		m_textbox.setString("");
}

void Textbox::SetFont(const sf::Font& font)
{
	m_textbox.setFont(font);
}

void Textbox::SetPosition(sf::Vector2f pos)
{
	m_textbox.setPosition(pos);
}

void Textbox::SetLimit(const bool& ToF)
{
	m_hasLimit = ToF;
}

void Textbox::SetLimit(const bool& ToF,const int& limit)
{
	m_hasLimit = ToF;
	m_limit = limit - 1;
}

void Textbox::SetSelected(const bool& sel)
{
	m_isSelected = sel;
	if (!sel)
	{
		std::string t = m_text.str();
		std::string newT = "";
		for (int i = 0; i < t.length(); i++)
		{
			newT += t[i];
		}
		m_textbox.setString(newT);
	}
}


void Textbox::SetText(const std::string& text)
{
	this->m_text << text;
}

std::string Textbox::GetText()const 
{
	return m_text.str();
}

void Textbox::DrawTo(sf::RenderTarget* target)
{
	target->draw(this->m_textbox);
}

void Textbox::Type(sf::Event input)
{
	if (m_isSelected)
	{
		int charTyped = input.text.unicode;
		if (charTyped < 128)
		{
			if (m_hasLimit)
			{
				if (m_text.str().length() <= m_limit)
					InputLogic(charTyped);
				else if (m_text.str().length() > m_limit && charTyped == DELETE_KEY)
					DeleteLastChar();

			}
			else
				InputLogic(charTyped);
		}
	}
}

Textbox& Textbox::operator=(const Textbox& t1)
{
	this->m_textbox = t1.m_textbox;
	std::string text = t1.GetText();
	this->m_textbox.setString(m_text.str());
	this->m_isSelected = t1.m_isSelected;
	this->m_hasLimit = t1.m_hasLimit;
	this->m_limit = t1.m_limit;
	return *this;
}

bool Textbox::IsSelected()
{
	return m_isSelected;
}

void Textbox::InputLogic(int charTyped)
{
	if (charTyped != DELETE_KEY && charTyped != ENTER_KEY && charTyped != ESCAPE_KEY)
	{
		m_text << static_cast<char>(charTyped);
	}
	 if (charTyped == DELETE_KEY)
	{
		if (m_text.str().length() > 0)
		{
			DeleteLastChar();
		}
	}
	 if (charTyped == ENTER_KEY)
	{
		if (m_text.str().length() > 0)
		{
			NewLine();
		}
	}
	m_textbox.setString(m_text.str() + "_");
}

void Textbox::DeleteLastChar()
{
	std::string t = m_text.str();
	std::string newT = "";
	for (int i = 0; i < t.length() - 1; i++)
	{
		newT += t[i];
	}
	m_text.str("");
	m_text << newT;
	m_textbox.setString(m_text.str());
}

void Textbox::NewLine()
{
	std::string t = m_text.str();
	t += "\n";
	m_text.str("");
	m_text << t;
	m_textbox.setString(m_text.str());
}
