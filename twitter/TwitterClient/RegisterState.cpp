#include "RegisterState.h"
#include <array>
#include <regex>
#include <iostream>
RegisterState::RegisterState(sf::RenderWindow* window, std::stack<State*>* states, std::shared_ptr<TcpSocket> socket)
	: State(window, states)
{
	mamaMia = socket;
	initButtons(window);
	initBox(window);
	m_enumStates = States::eRegisterState;
}

RegisterState::~RegisterState()
{
	delete this->m_button.Back;
	delete this->m_button.CreateAccount;
	delete this->m_button.Bio;
	delete this->m_button.Birthday;
	delete this->m_button.Username;
	delete this->m_button.Name;
	delete this->m_button.Website;
	delete this->m_button.Location;

	delete this->m_textBox.Username;
	delete this->m_textBox.Name;
	delete this->m_textBox.Bio;
	delete this->m_textBox.Birthday;
	delete this->m_textBox.Website;
	delete this->m_textBox.Location;

	m_enumStates = States::eLoginState;
}

void RegisterState::endState()
{
	std::cout << "Ending RegisterState\n";
}

void RegisterState::updateKeybinds(const float& dt)
{
	this->checkforQuit();
}

void RegisterState::updateButtons()
{
	this->m_button.Back->update(m_mousePosView);
	this->m_button.CreateAccount->update(m_mousePosView);

	this->m_button.Username->update(m_mousePosView);
	this->m_button.Bio->update(m_mousePosView);
	this->m_button.Birthday->update(m_mousePosView);
	this->m_button.Location->update(m_mousePosView);
	this->m_button.Website->update(m_mousePosView);
	this->m_button.Name->update(m_mousePosView);
	
}

void RegisterState::updateUseButtons()
{
	if (this->m_button.Back->getButtonState() == 2 and m_buttonRelease == 1)
	{
			//action
			this->m_states->pop();
			std::cout << "backRegister\n";
			m_buttonRelease = 0;
		
	}
	if (this->m_button.CreateAccount->getButtonState()==2 and m_buttonRelease == 1)
	{
			ClientFeatures::Network en = ClientFeatures::Register;

			uint16_t aux = static_cast<uint16_t>(en);
			std::cout << aux << '\n';
			if (regex_match(m_textBox.Birthday->GetText(), std::regex("(0[1-9]|1[0-9]|2[0-9]|3[0-1])(-)(0[1-9]|1[0-2])(-)(19[0-9][0-9]|20[0-9][0-9])")))
			{
				bool result = mamaMia->Send((char*)&aux, sizeof(char));
				if (result)
				{
					bool breakFor = false;

					for (int i = 0; i < 6; i++)
					{
						if (breakFor == true)
						{
							break;
						}
						std::string  received;
						std::string message;
						switch (i)
						{
						case 0:
						{

							message = m_textBox.Username->GetText();
							std::cout << message << '\n';
							mamaMia->Send(message.c_str(), message.size());

							std::array<char, 512> receiveBuffer;
							int revieved;
							mamaMia->Receive(receiveBuffer.data(), receiveBuffer.size(), revieved);

							std::copy(receiveBuffer.begin(), receiveBuffer.begin() + revieved, std::back_inserter(received));

							if (received.compare("Username already taken") == 0)
								// message box cu username is already taken
							{
								breakFor = true;
							}

							break;
						}
						case 1:
						{
							message = m_textBox.Name->GetText();
							std::cout << message << '\n';
							mamaMia->Send(message.c_str(), message.size());

							std::array<char, 512> receiveBuffer;
							int revieved;
							mamaMia->Receive(receiveBuffer.data(), receiveBuffer.size(), revieved);

							std::copy(receiveBuffer.begin(), receiveBuffer.begin() + revieved, std::back_inserter(received));

							if (received.compare("ok") != 0)
								breakFor = true;
							break;
						}
						case 2:
						{
							message = m_textBox.Bio->GetText();
							std::cout << message << '\n';
							mamaMia->Send(message.c_str(), message.size());

							std::array<char, 512> receiveBuffer;
							int revieved;
							mamaMia->Receive(receiveBuffer.data(), receiveBuffer.size(), revieved);

							std::copy(receiveBuffer.begin(), receiveBuffer.begin() + revieved, std::back_inserter(received));

							if (received.compare("ok") != 0)
								breakFor = true;
							break;
						}
						case 3:
						{

							message = m_textBox.Birthday->GetText();
							std::cout << message << '\n';
							mamaMia->Send(message.c_str(), message.size());

							std::array<char, 512> receiveBuffer;
							int revieved;
							mamaMia->Receive(receiveBuffer.data(), receiveBuffer.size(), revieved);

							std::copy(receiveBuffer.begin(), receiveBuffer.begin() + revieved, std::back_inserter(received));

							if (received.compare("ok") != 0)
								breakFor = true;
							break;
						}
						case 4:
						{

							message = m_textBox.Website->GetText();
							std::cout << message << '\n';
							mamaMia->Send(message.c_str(), message.size());

							std::array<char, 512> receiveBuffer;
							int revieved;
							mamaMia->Receive(receiveBuffer.data(), receiveBuffer.size(), revieved);

							std::copy(receiveBuffer.begin(), receiveBuffer.begin() + revieved, std::back_inserter(received));

							if (received.compare("ok") != 0)
								breakFor = true;
							break;
						}
						case 5:
						{

							message = m_textBox.Location->GetText();
							std::cout << message << '\n';
							mamaMia->Send(message.c_str(), message.size());

							std::array<char, 512> receiveBuffer;
							int revieved;
							mamaMia->Receive(receiveBuffer.data(), receiveBuffer.size(), revieved);

							std::copy(receiveBuffer.begin(), receiveBuffer.begin() + revieved, std::back_inserter(received));

							if (received.compare("ok") != 0)
								breakFor = true;
							break;
						}
						}
					}
					if (breakFor == true)
						std::cout << "Account Register Failed!\n";

				}
			}
			else
				std::cout << "The birthday is not in dd-mm-yyyy format\n";
			std::cout << "accountRegister\n";
			m_buttonRelease = 0;
	}
	if (this->m_button.Bio->getButtonState() == 2)
	{
		this->m_registerPointer->SetSelected(false);
		this->m_textBox.Bio->SetSelected(true);
		this->m_registerPointer = this->m_textBox.Bio;
	}
	if (this->m_button.Username->getButtonState() == 2)
	{
		this->m_registerPointer->SetSelected(false);
		this->m_textBox.Username->SetSelected(true);
		m_registerPointer = this->m_textBox.Username;
	}
	if (this->m_button.Name->getButtonState() == 2)
	{
		this->m_registerPointer->SetSelected(false);
		this->m_textBox.Name->SetSelected(true);
		this->m_registerPointer = this->m_textBox.Name;
	}
	if (this->m_button.Birthday->getButtonState() == 2)
	{
		this->m_registerPointer->SetSelected(false);
		this->m_textBox.Birthday->SetSelected(true);
		this->m_registerPointer = this->m_textBox.Birthday;
	}
	if (this->m_button.Website->getButtonState() == 2)
	{
		this->m_registerPointer->SetSelected(false);
		this->m_textBox.Website->SetSelected(true);
		this->m_registerPointer = this->m_textBox.Website;
	}
	if (this->m_button.Location->getButtonState() == 2)
	{
		this->m_registerPointer->SetSelected(false);
		this->m_textBox.Location->SetSelected(true);
		this->m_registerPointer = this->m_textBox.Location;
	}
}

void RegisterState::update(const float& dt)
{
	this->updateButtons();
	this->updateUseButtons();
	this->updateMousePos();
	this->updateKeybinds(dt);
}

void RegisterState::SetText(const sf::Event& input)
{
	if(this->m_textBox.Username->IsSelected())
		this->m_textBox.Username->Type(input);
	if (this->m_textBox.Name->IsSelected())
		this->m_textBox.Name->Type(input);
	if (this->m_textBox.Bio->IsSelected())
		this->m_textBox.Bio->Type(input);
	if (this->m_textBox.Birthday->IsSelected())
		this->m_textBox.Birthday->Type(input);
	if (this->m_textBox.Website->IsSelected())
		this->m_textBox.Website->Type(input);
	if (this->m_textBox.Location->IsSelected())
		this->m_textBox.Location->Type(input);
}


void RegisterState::renderButtons(sf::RenderTarget* target)
{
	this->m_button.Back->render(target);
	this->m_button.CreateAccount->render(target);
	this->m_button.Username->render(target);
	this->m_textBox.Username->DrawTo(target);
	this->m_button.Name->render(target);
	this->m_textBox.Name->DrawTo(target);
	this->m_button.Bio->render(target);
	this->m_textBox.Bio->DrawTo(target);
	this->m_button.Birthday->render(target);
	this->m_textBox.Birthday->DrawTo(target);
	this->m_button.Website->render(target);
	this->m_textBox.Website->DrawTo(target);
	this->m_button.Location->render(target);
	this->m_textBox.Location->DrawTo(target);

	this->m_labels.Username->render(target);
	this->m_labels.Name->render(target);
	this->m_labels.Bio->render(target);
	this->m_labels.Birthday->render(target);
	this->m_labels.Website->render(target);
	this->m_labels.Location->render(target);
}

void RegisterState::renderBox(sf::RenderTarget* target)
{
	target->draw(m_registerBox);
}

void RegisterState::render(sf::RenderTarget* target)
{
	if (!target)
		target = this->m_window;

	renderBox(target);
	renderButtons(target);
}

void RegisterState::initButtons(sf::RenderTarget* target)
{
	this->m_button.Back = new Button(target->getSize().x / 3 - target->getSize().x / 16, target->getSize().y / 3 * 2.5 - target->getSize().y / 40,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "Back",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue, this->m_color.Black);
	this->m_button.CreateAccount = new Button(target->getSize().x / 3 * 2 - target->getSize().x / 16, target->getSize().y / 3 * 2.5 - target->getSize().y / 40,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "Create",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue, this->m_color.Black);


	this->m_button.Username = new Button(target->getSize().x / 3 - target->getSize().x / 16, target->getSize().y / 6.5 - target->getSize().y / 40,
		target->getSize().x / 2.2, target->getSize().y / 20, &this->m_font, "",
		this->m_color.Blue, this->m_color.DarkGray,
		this->m_color.Blue,sf::Color::Transparent);
	this->m_textBox.Username = new Textbox(15, this->m_color.Black, true);
	this->m_textBox.Username->SetFont(m_font);
	float x, y;
	x = target->getSize().x / 3 - target->getSize().x / 16;
	y = target->getSize().y / 6.5 - target->getSize().y / 40;
	this->m_textBox.Username->SetPosition({ x, y });

	this->m_registerPointer = m_textBox.Username;

	this->m_labels.Username = new Button(target->getSize().x / 4 - target->getSize().x / 16, target->getSize().y / 6.5 - target->getSize().y / 40, 20, 20, &this->m_font,
		"Username: ", this->m_color.Black, this->m_color.DarkGray, this->m_color.DarkGray, sf::Color::Transparent);



	this->m_button.Name = new Button(target->getSize().x / 3 - target->getSize().x / 16, target->getSize().y / 4 - target->getSize().y / 40,
		target->getSize().x / 2.2, target->getSize().y / 20, &this->m_font, "",
		this->m_color.Blue, this->m_color.DarkGray,
		this->m_color.Blue, sf::Color::Transparent);

	this->m_textBox.Name = new Textbox(15, this->m_color.Black, false);
	this->m_textBox.Name->SetFont(m_font);
	x = target->getSize().x / 3 - target->getSize().x / 16;
	y = target->getSize().y / 4 - target->getSize().y / 40;
	this->m_textBox.Name->SetPosition({ x, y });

	this->m_labels.Name = new Button(target->getSize().x / 4 - target->getSize().x / 16, target->getSize().y / 4 - target->getSize().y / 40, 20, 20, &this->m_font,
		"Name: ", this->m_color.Black, this->m_color.DarkGray, this->m_color.DarkGray, sf::Color::Transparent);

	this->m_button.Bio = new Button(target->getSize().x / 3 - target->getSize().x / 16, target->getSize().y / 2.5 - target->getSize().y / 40,
		target->getSize().x / 2.2, target->getSize().y / 20, &this->m_font, "",
		this->m_color.Blue, this->m_color.DarkGray,
		this->m_color.Blue, sf::Color::Transparent);

	this->m_textBox.Bio = new Textbox(15, this->m_color.Black, false);
	this->m_textBox.Bio->SetFont(m_font);
	x = target->getSize().x / 3 - target->getSize().x / 16;
	y = target->getSize().y / 2.5 - target->getSize().y / 40;
	this->m_textBox.Bio->SetPosition({ x, y });

	this->m_labels.Bio = new Button(target->getSize().x / 4 - target->getSize().x / 16, target->getSize().y / 2.5 - target->getSize().y / 40, 20, 20, &this->m_font,
		"Bio: ", this->m_color.Black, this->m_color.DarkGray, this->m_color.DarkGray, sf::Color::Transparent);

	// model birthday : dd-mm-yyyy
	// (0[1-9]|1[0-9]| 2[0-9]|3[0-1])(-)(0[1-9]|1[0-2])(-)(19[0-9][0-9]|20[0-9][0-9])
	this->m_button.Birthday = new Button(target->getSize().x / 3 - target->getSize().x / 16, target->getSize().y / 2 - target->getSize().y / 40,
		target->getSize().x / 2.2, target->getSize().y / 20, &this->m_font, "",
		this->m_color.Blue, this->m_color.DarkGray,
		this->m_color.Blue, sf::Color::Transparent);

	this->m_textBox.Birthday = new Textbox(15, this->m_color.Black, false);
	this->m_textBox.Birthday->SetFont(m_font);
	x = target->getSize().x / 3 - target->getSize().x / 16;
	y = target->getSize().y / 2 - target->getSize().y / 40;
	this->m_textBox.Birthday->SetPosition({ x, y });

	this->m_labels.Birthday = new Button(target->getSize().x / 4 - target->getSize().x / 16, target->getSize().y / 2 - target->getSize().y / 40, 20, 20, &this->m_font,
		"Birthdate: ", this->m_color.Black, this->m_color.DarkGray, this->m_color.DarkGray, sf::Color::Transparent);

	this->m_button.Website = new Button(target->getSize().x / 3 - target->getSize().x / 16, target->getSize().y / 1.5 - target->getSize().y / 40,
		target->getSize().x / 2.2, target->getSize().y / 20, &this->m_font, "",
		this->m_color.Blue, this->m_color.DarkGray,
		this->m_color.Blue, sf::Color::Transparent);

	this->m_textBox.Website = new Textbox(15, this->m_color.Black, false);
	this->m_textBox.Website->SetFont(m_font);
	x = target->getSize().x / 3 - target->getSize().x / 16;
	y = target->getSize().y / 1.5 - target->getSize().y / 40;
	this->m_textBox.Website->SetPosition({ x, y });

	this->m_labels.Website = new Button(target->getSize().x / 4 - target->getSize().x / 16, target->getSize().y / 1.5 - target->getSize().y / 40, 20, 20, &this->m_font,
		"Website: ", this->m_color.Black, this->m_color.DarkGray, this->m_color.DarkGray, sf::Color::Transparent);

	this->m_button.Location = new Button(target->getSize().x / 3 - target->getSize().x / 16, target->getSize().y / 1.3 - target->getSize().y / 40,
		target->getSize().x / 2.2, target->getSize().y / 20, &this->m_font, "",
		this->m_color.Blue, this->m_color.DarkGray,
		this->m_color.Blue, sf::Color::Transparent);

	this->m_textBox.Location = new Textbox(15, this->m_color.Black, false);
	this->m_textBox.Location ->SetFont(m_font);
	x = target->getSize().x / 3 - target->getSize().x / 16;
	y = target->getSize().y / 1.3 - target->getSize().y / 40;
	this->m_textBox.Location->SetPosition({ x, y });

	this->m_labels.Location = new Button(target->getSize().x / 4 - target->getSize().x / 16, target->getSize().y / 1.3 - target->getSize().y / 40, 20, 20, &this->m_font,
		"Location: ", this->m_color.Black, this->m_color.DarkGray, this->m_color.DarkGray, sf::Color::Transparent);
}

void RegisterState::initBox(sf::RenderTarget* target)
{
	this->m_registerBox = sf::RectangleShape(sf::Vector2f(target->getSize().x / 12 *10, target->getSize().y / 12 * 10));
	this->m_registerBox.setPosition(sf::Vector2f(target->getSize().x / 12, target->getSize().y / 12));
	this->m_registerBox.setFillColor(this->m_color.Black);
	this->m_registerBox.setOutlineColor(this->m_color.DarkGray);
	this->m_registerBox.setOutlineThickness(-2);
}
