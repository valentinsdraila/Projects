#include "State.h"

State::States State::getCurrentState() const
{
	return m_enumStates;
}

State::State(sf::RenderWindow* window, std::stack<State*> *states)
{
	this->m_window = window;
	this->m_states = states;
	this->quit = false;
	this->m_buttonRelease = 1;
	this->initFont();
	this->initColor();

}

State::~State()
{
}

const bool& State::getQuit() const
{
	return this->quit;
}

void State::checkforQuit()
{
	if (sf::Keyboard::isKeyPressed(sf::Keyboard::Escape))
		this->quit = true;
}


void State::btnReleased()
{
	m_buttonRelease = 1;
}



void State::updateMousePos()
{
	this->m_mousePosView = this->m_window->mapPixelToCoords(sf::Mouse::getPosition(*this->m_window));
}


void State::initColor()
{
	this->m_color.Blue = sf::Color(0xDA1F2FF);
	this->m_color.Black = sf::Color(0x14171AFF);
	this->m_color.DarkGray = sf::Color(0x657786FF);
	this->m_color.LightGray = sf::Color(0xAAB8C2FF);
	this->m_color.ExtraLightGray = sf::Color(0xE1E8EDFF);
	this->m_color.ExtraExtraLightGray = sf::Color(0xF5F8FAFF);
}


void State::initFont()
{
	this->m_font.loadFromFile("arial.ttf");
}


void State::SetNetworkEnum(const ClientFeatures::Network& network)
{
	this->m_network = network;
}
