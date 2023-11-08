#pragma once

namespace ClientFeatures 
{
	//enum pentru functionalitati

	enum Network : uint16_t
	{
		Register = 0,
		Login = 1,
		MakePost = 2,
		ShowFollowers = 3,
		Like = 4,
		Exit = 5,
		AddFriend = 6,
		Profile = 7,
		NextPost = 8,
		PreviousPost = 9,
		ProfileNextPost = 10,
		ProfilePreviousPost = 11,
		Refresh = 12
		//....
	};
}
