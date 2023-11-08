#pragma once
#include <vector>
#include <iostream>
#include <chrono>

/// <summary>
/// This PriorityQueue works if date has the same format as :yyyy/mm/dd
/// </summary>
/// <typeparam name="T"></typeparam>

template <typename T>
class PriorityQueue
{
	
	private:
		std::vector<T> m_data;
	public:
		~PriorityQueue() = default;
		PriorityQueue() = default;
		PriorityQueue(int size) {
			m_data.reserve(size);
		}
		int Size()const {
			return m_data.size();
		}
		void Insert(const T& value);
		void ExtractMax();
		T GetMaxElement()const; 
		void IncreaseKey(int position, const T& value);
		void MaxHeapfy(const int& position);
		void Display();
};

template <typename T>
inline void PriorityQueue<T>::Insert(const T& value)
{
	if (std::is_same<T, int>::value || std::is_same<T, double>::value)
		m_data.push_back(0);
	if (std::is_same<T, std::string>::value)
		m_data.push_back("\0");
	int size = m_data.size();
	IncreaseKey(size - 1, value);
}

template <typename T>
inline T PriorityQueue<T>::GetMaxElement() const
{
	return m_data[0];
}

template <typename T>
inline void PriorityQueue<T>::ExtractMax()
{
	int size = m_data.size();
	m_data[0] = m_data[size - 1];
	m_data.pop_back();
	MaxHeapfy(0);
}

template <typename T>
inline void PriorityQueue<T>::IncreaseKey(int position, const T& value)
{
	if (value > m_data[position])
	{
		m_data[position] = value;
		int p = (position - 1) / 2;
		while (position > 0 && m_data[p] < value)
		{
			m_data[position] = m_data[p];
			position = p;
			p = (position - 1) / 2;
		}
		m_data[position] = value;
	}
}

template <typename T>
inline void PriorityQueue<T>::MaxHeapfy(const int& position)
{
	int st = 2 * position + 1;
	int dr = 2 * position + 2;
	int positionMax = position;
	int size = m_data.size();
	if (st<size && m_data[st]>m_data[positionMax])
		positionMax = st;
	if (dr<size && m_data[dr]>m_data[positionMax])
		positionMax = dr;
	if (positionMax != position)
	{
		auto aux = m_data[position];
		m_data[position] = m_data[positionMax];
		m_data[positionMax] = aux;
		MaxHeapfy(positionMax);
	}
}

template <typename T>
inline void PriorityQueue<T>::Display()
{
	for (const auto& x : m_data)
		std::cout << static_cast<T>(x) << " ";
	std::cout << '\n';
}


