   M S B A M L   `   `   `  ����B  >Hangman, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Hangman.ViewHangman.View  5   	   5clr-namespace:Hangman.ViewModelHangman.ViewModel  62       Hangman.View.GameWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Hangman.View     5   	   5	viewmodelclr-namespace:Hangman.ViewModel     62     � Title$  
GameWindow��5   	      	LetterKey � DataContext# [  6   $	��750��6E   $
��1350��6R   .��5
      � -   5         �Name$ GameGrid��5
      .+�5   
   �� $��New Game��6   $��Right=�6$   $	��100��6@   $��50��6L   ��6X   �� *	NewGame+ �� Mode$ OneWay=��� 5   
   $
��Left=�67   $��Bottom=�6R   $	��100��6m   ��6   �� *	Letters+��5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6!   5      ��5      �� 5      .R�5      �� $��50��6�   $��50��6�   ��6   �� *Letter+��68   �� *LetterPressed+$ OneWay=� �� Source# [  ��6�   �� {�6�   �� *Enable+6�   5      5      �� 5   
   $
��Left=�64   $	��Top=�6O   $	��100��6g   ��6   �� *Word+��5      �� 5      .R�5      �� $ 
Horizontal=�6!   5      ��5      �� 5      .R�5       �� $�� ��6   ${�False��6(   $��50��6:   $��50��6E   6S   5!      5#      �� 5$   
   $
��Left=�64   $	��Top=�6O   $	��100��6g   ��6   �� *Word+��5%      �� 5&      .R�5'      �� $ 
Horizontal=�6!   5(      ��5*      �� 5+      .R�5,      �� $��30(�68   $��50��6f   $��50��6q   ��6    �� *Letter+y�6F   �� *	Visible+6q   5-      5/      �� 50   
   $	��200��6=   $	��200��6I   $��713,35,437,199q�6V   ��6   �� *CurrentImage+$ TwoWay=�6p   �� 51   
   $��Lives left:��6   $��30(�6I   $	��200��6W   $	��100��6c   $��1061,50,89,284q�6p   y�6'   �� *
Visibility+6�   �� 52   
   $��30(�6M   $	��200��6[   $	��100��6g   $��1183,50,-33,284q�6t   ��6   �� *Lives+y�6+   �� *
Visibility+6�   �� 53   
   $��30(�6M   $	��200��6[   $	��100��6g   $��1045,185,105,149q�6t   ��6   �� *Delay+y�6+   �� *
Visibility+6�   54      55      