#include "TweetState.h"

TweetState::TweetState(sf::RenderWindow* window, std::stack<State*>* states, std::shared_ptr<TcpSocket> mamaMia)
	: State(window, states)
{
	this->mamaMia =mamaMia;
	initBox(window);
	initButtons(window);
	initLines(window);
	initTextbox(window);
	m_enumStates = States::eTweetState;
}

TweetState::~TweetState()
{
	delete this->m_button.Back;
	delete this->m_button.Tweet;

	delete this->m_tweetTextbox;
}

void TweetState::endState()
{
	std::cout << "Ending TweetState\n";
}

void TweetState::updateKeybinds(const float& dt)
{
	this->checkforQuit();
}

void TweetState::updateButtons()
{
	this->m_button.Back->update(m_mousePosView);
	this->m_button.Tweet->update(m_mousePosView);
}

void TweetState::updateUseButtons()
{
	if (this->m_button.Back->getButtonState() == 2 and m_buttonRelease == 1)
	{	
			this->m_states->pop();
			std::cout << "backTweet";
			m_buttonRelease = 0;
		
	}
	if (this->m_button.Tweet->getButtonState() == 2 and m_buttonRelease == 1)
	{		
			//send info
			std::string message = m_tweetTextbox->GetText();

			ClientFeatures::Network en = ClientFeatures::MakePost;

			uint16_t aux = static_cast<uint16_t>(en);
			std::cout << aux << '\n';


			bool result = mamaMia->Send((char*)&aux, sizeof(char));
			//se duce inapoi in feed
			if (result)
			{
				std::string message = this->m_tweetTextbox->GetText();

				mamaMia->Send(message.c_str(), message.size());

			}
			this->m_states->pop();
			std::cout << "btnTweet\n";
			m_buttonRelease = 0;
	
	}
}

void TweetState::update(const float& dt)
{
	this->updateButtons();
	this->updateUseButtons();
	this->updateMousePos();
	this->updateKeybinds(dt);
}

void TweetState::SetText(const sf::Event& input)
{
	if (this->m_tweetTextbox->IsSelected())
		this->m_tweetTextbox->Type(input);
}

void TweetState::renderButtons(sf::RenderTarget* target)
{
	this->m_button.Back->render(target);
	this->m_button.Tweet->render(target);
}

void TweetState::renderBox(sf::RenderTarget* target)
{
	target->draw(m_tweetBox);
}

void TweetState::renderLines(sf::RenderTarget* target)
{
	target->draw(m_downLine);
}

void TweetState::renderTextbox(sf::RenderTarget* target)
{
	this->m_tweetTextbox->DrawTo(target);
}

void TweetState::render(sf::RenderTarget* target)
{
	if (!target)
		target = this->m_window;

	renderBox(target);
	renderLines(target);
	renderButtons(target);
	renderTextbox(target);
}

void TweetState::initButtons(sf::RenderTarget* target)
{
	this->m_button.Back = new Button(target->getSize().x / 12 + 10, target->getSize().y / 12+ 10,
		target->getSize().x / 8, target->getSize().y / 20,
		&this->m_font, "Back",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue, this->m_color.Black);
	this->m_button.Tweet = new Button(target->getSize().x / 3, target->getSize().y /5*4-20,
		target->getSize().x / 3, target->getSize().y /10, &this->m_font, "Tweet",
		this->m_color.Blue, this->m_color.DarkGray,
		this->m_color.ExtraExtraLightGray, this->m_color.Black);
}

void TweetState::initBox(sf::RenderWindow* target)
{
	this->m_tweetBox = sf::RectangleShape(sf::Vector2f(target->getSize().x / 12 * 10, target->getSize().y / 12 * 10));
	this->m_tweetBox.setPosition(sf::Vector2f(target->getSize().x / 12, target->getSize().y / 12));
	this->m_tweetBox.setFillColor(this->m_color.Black);
	this->m_tweetBox.setOutlineColor(this->m_color.DarkGray);
	this->m_tweetBox.setOutlineThickness(1);
}

void TweetState::initLines(sf::RenderWindow* target)
{
	this->m_downLine = sf::VertexArray(sf::Lines, 2);
	this->m_downLine[0] = sf::Vector2f(target->getSize().x / 12*2, target->getSize().y / 5 * 4 - 40), m_color.DarkGray;
	this->m_downLine[1] = sf::Vector2f(target->getSize().x / 12*10, target->getSize().y / 5 * 4 - 40), m_color.DarkGray;
	this->m_downLine[0].color = this->m_color.DarkGray;
	this->m_downLine[1].color = this->m_color.DarkGray;
}


void TweetState::initTextbox(sf::RenderWindow* target)
{
	float x, y;
	this->m_tweetTextbox = new Textbox(20, this->m_color.ExtraExtraLightGray, true);
	this->m_tweetTextbox->SetFont(m_font);
	this->m_tweetTextbox->SetLimit(1, 140);
	x = target->getSize().x / 12 * 2;
	y = target->getSize().y / 5 * 3;
	this->m_tweetTextbox->SetPosition({ x,y });
}
