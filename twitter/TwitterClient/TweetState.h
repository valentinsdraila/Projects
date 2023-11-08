#pragma once
#include "State.h"
#include "FeedState.h"
class TweetState : public State
{
private:
	struct TwitterTweetButtons {
		Button* Back;
		Button* Tweet;
	};
	TwitterTweetButtons m_button;

	sf::RectangleShape m_tweetBox;
	sf::VertexArray m_downLine;
	Textbox* m_tweetTextbox;

	std::shared_ptr<TcpSocket> mamaMia;
public:
	TweetState(sf::RenderWindow* window, std::stack<State*>* states, std::shared_ptr<TcpSocket> mamaMia);
	virtual ~TweetState();


	void endState();
	void updateKeybinds(const float& dt);
	void updateButtons();
	void updateUseButtons();
	void update(const float& dt);

	void SetText(const sf::Event& input);

	void renderButtons(sf::RenderTarget* target);
	void renderBox(sf::RenderTarget* target);
	void renderLines(sf::RenderTarget* target);
	void renderTextbox(sf::RenderTarget* target);
	void render(sf::RenderTarget* target = NULL);


	void initButtons(sf::RenderTarget* target);
	void initBox(sf::RenderWindow* target);
	void initLines(sf::RenderWindow* target);
	void initTextbox(sf::RenderWindow* target);


};
