   M S B A M L   `   `   `  ����B  >Hangman, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Hangman.ViewModelHangman.ViewModel  5   P   -clr-namespace:Hangman.ModelHangman.Model  6�       Hangman.MainWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5	viewmodelclr-namespace:Hangman.ViewModel     6P   -modelclr-namespace:Hangman.Model     6�     � Title$  
MainWindow��5   	   $	��550��6   $	��900��6)    � DataContext 5      %  Hangman.ViewModel.MainViewModel  5	   
   .��5      � 
�������.+�5   
   �� -   5        $��Picture��5      $��418,55,59,64q�6E   $	��250��6[   ��6   �� *SelectedUser.image+5      �� 5   
   -   5         ��Name$
 list��5      $	��300��6r   $	��200��6   $
��Left=�6�   $	��Top=�6�   $��40, 50, 0, 0q�6�   ��6    �� *Users+ �� SelectedItem 6>   �� *SelectedUser+ �� Mode$ TwoWay=� �� SelectedIndex 6�   �� *SelectedName+��5      �� 5      .R�5      �� ��6%   �� *
username+6D   5      5      �� 5   
   $	��100��6   $��50��6   $��New User��6)   $��90,355,610,-1q�6<   ��6S   ��  �� Path$ 
NewCommand�6y   �� 5   
   $	��100��6   $��50��6   $��Delete User��6)   $��251,370,449,14q�6?   {�6W   �� *EnableButton+��6z   �� $ DeleteCommand�6�   �� 5   
   $	��100��6   $��50��6   $
��Play��6)   $��412,370,288,14q�68   {�6P   �� *EnableButton+��6s   �� $ PlayCommand�6�   �� 5   
   -   5        $	��100��5      $��50��6   $��Cancel��6)   $��583,370,117,14q�6:   6j   5      5      