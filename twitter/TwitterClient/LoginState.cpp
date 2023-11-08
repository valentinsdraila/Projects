#include "LoginState.h"

LoginState::LoginState(sf::RenderWindow* window, std::stack<State*>* states, std::shared_ptr<TcpSocket> socket)
	: State(window, states)
{
	mamaMia = socket;
	initButtons(window);
	m_count = 0;
	m_enumStates = States::eLoginState;
}

LoginState::~LoginState()
{
	delete this->m_button.Register;
	delete this->m_button.Login;
	delete this->m_button.TextBoxButton;
	delete this->m_textbox;
}

void LoginState::endState()
{
	std::cout << "Ending LoginState\n";
}

void LoginState::updateKeybinds(const float& dt)
{
	this->checkforQuit();
}

void LoginState::updateButtons()
{
	this->m_button.Register->update(m_mousePosView);
	this->m_button.Login->update(m_mousePosView);
	this->m_button.TextBoxButton->update(m_mousePosView);
	
}

void LoginState::updateUseButtons()
{
	if (this->m_button.Register->getButtonState() == 2 and m_buttonRelease == 1)
	{
		
			//action
			this->m_states->push(new RegisterState(this->m_window, this->m_states, mamaMia));
			std::cout << "salut de la Register\n";
			m_buttonRelease = 0;

	}
	if (this->m_button.Login->getButtonState() == 2 and m_buttonRelease == 1)
	{
	
			std::string username;
			ClientFeatures::Network en = ClientFeatures::Login;

			uint16_t aux = static_cast<uint16_t>(en);
			std::cout << aux << '\n';


			bool result = mamaMia->Send((char*)&aux, sizeof(char));
			if (result)
			{
				username = this->m_textbox->GetText();
				std::cout << username << "\n";
				mamaMia->Send(username.c_str(), username.size());

				std::array<char, 512> rBuffer;
				int revieved;
				std::string recieved;
				mamaMia->Receive(rBuffer.data(), rBuffer.size(), revieved);

				std::copy(rBuffer.begin(), rBuffer.begin() + revieved, std::back_inserter(recieved));

				if (recieved.compare("Login Failed") == 0)
				{
					///There is no user with this name
					std::cout << "There is no user with this name\n";
				}
				else
				{
					this->m_states->push(new FeedState(this->m_window, this->m_states, mamaMia));
					std::cout << "salut de la Login\n";
				}
				m_buttonRelease = 0;

			}
		
	}
	if (this->m_button.TextBoxButton->getButtonState() == 2)
	{
		m_textbox->SetSelected(true);
	}
}

void LoginState::update(const float& dt)
{
	/**
	sf::Text textulescu;
	textulescu.setString("Username: ");
	textulescu.setPosition({ 50, 50 });
	textulescu.setFillColor(sf::Color::Blue);
	textulescu.setFont(this->m_font);
	
	*/

	this->updateButtons();
	if(m_count%9==0)
	this->updateUseButtons();
	this->updateMousePos();
	this->updateKeybinds(dt);
	m_count++;
	
}

void LoginState::SetText(sf::Event input)
{
	m_textbox->Type(input);
}


void LoginState::renderButtons(sf::RenderTarget* target)
{
	this->m_button.Register->render(target);
	this->m_button.Login->render(target);
	this->m_button.TextBoxButton->render(target);
	this->m_textbox->DrawTo(target);

	target->draw(m_twitterLogo);
}


void LoginState::render(sf::RenderTarget* target)
{
	if (!target)
		target = this->m_window;

	renderButtons(target);
}

void LoginState::initButtons(sf::RenderTarget* target)
{
	this->m_button.Register = new Button(target->getSize().x/3-target->getSize().x/16,target->getSize().y/3*2-target->getSize().y/40,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "Register",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue, this->m_color.Black);
	this->m_button.Login = new Button(target->getSize().x/3*2- target->getSize().x / 16,target->getSize().y/3*2 - target->getSize().y / 40,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "Login",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue, this->m_color.Black);
	this->m_button.TextBoxButton = new Button(target->getSize().x / 3 - target->getSize().x / 16, target->getSize().y / 2  - target->getSize().y / 40,
		target->getSize().x / 2.2, target->getSize().y / 20, &this->m_font, "", this->m_color.Blue, this->m_color.DarkGray,
		this->m_color.Blue, this->m_color.Black);

	this->m_textbox = new Textbox( 15, this->m_color.Black, true);
	this->m_textbox->SetFont(m_font);
	float x, y;
	x = target->getSize().x / 3 - target->getSize().x / 16;
	y = target->getSize().y / 2 - target->getSize().y / 40;
	this->m_textbox->SetPosition({ x, y });
	this->m_textbox->SetSelected(true);

	this->m_twitterImg.loadFromFile("../images2/twitter.jpg");
	this->m_twitterImg.setSmooth(true);
	this->m_twitterLogo.setScale(sf::Vector2f(0.30, 0.30));
	this->m_twitterLogo.setPosition(sf::Vector2f(target->getSize().x/2-200,
		target->getSize().y/5));
	this->m_twitterLogo.setTexture(m_twitterImg);

	
	
}
