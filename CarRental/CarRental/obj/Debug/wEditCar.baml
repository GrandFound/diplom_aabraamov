   M S B A M L   `   `   `  ����D  @CarRental, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:CarRental	CarRental  5   	       CarRental.wEditCar   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:CarRental     5   	     � Title$7  1Редактирование автомобиля��5   	   $	��450��6+   $	��800��68    � WindowStartupLocation$ CenterScreen=�6D   .��5	      �    Back#��[  6   .+�5
   
   � $��Center=�6    � Orientation$ Vertical=�61   .+�5      �� $��Center=�6   $��25q�65   $��24(�6A   31Редактирование автомобиля6j   � 5      $ 
Horizontal=�6   .+�5      � $ Vertical=�6   $��20q�63   .+�5      � $ 
Horizontal=�6    .+�5      �� $	��170��6    $��Марка машины��6,   �� 5      -   5        $��	chosenCar��5   $   ��65   �� *CarBrands.BrandName+5      � 5      $
��Left=�6    $ 
Horizontal=�6;   .+�5      �� $	��170��6    $/��)Регистрационный номер��6,   �� 5      -   5         ��Name$ RegistrationNumber��5   "   ��6>   �� *RegistrationNumber+5      � 5      $
��Left=�6    $ 
Horizontal=�6;   .+�5      �� $	��170��6    $��Номер кузова��6,   �� 5      -   5        $ 
BodyNumber��5   "   ��66   �� *
BodyNumber+5      � 5      $
��Left=�6    $ 
Horizontal=�6;   .+�5      �� $	��170��6    $#��Номер двигателя��6,   �� 5      -   5        $ EngineNumber��5   "   ��68   �� *EngineNumber+5      � 5      $
��Left=�6    $ 
Horizontal=�6;   .+�5       �� $	��170��6    $#��Фото автомобиля��6,   �� 5!      -   5         ��Name$ addPhotoButton��5!   !   $��Выбрать��69   6K   5"      5#      � 5%      $ Vertical=�6   .+�5&      � $ 
Horizontal=�6    .+�5'      �� $	��150��6    $��Год выпуска��6,   �� 5(      -   5        $ YearOfIssue��5(   "   ��67   �� *YearOfIssue+5)      � 5*      $ 
Horizontal=�6    .+�5+      �� $	��150��6    $#��Цена автомобиля��6,   �� 5,      -   5        $ CarPrice��5,   "   ��64   �� *
CarPrice+5-      � 5.      $ 
Horizontal=�6    .+�5/      �� $	��150��6    $��Цена за день��6,   �� 50      -   5        $ RentalDayPrice��50   "   ��6:   �� *RentalDayPrice+51      � 52      $ 
Horizontal=�6    .+�53      �� $	��150��6    $(��"Дата последнего ТО��6,   (  "System.Windows.Controls.DatePicker  55      -	   5          Name$ DateOfLastMaintenance��55   %      SelectedDateFormat$ Short=�6s      SelectedDate 6D   �� *DateOfLastMaintenance+6s   56      � 57      $ 
Horizontal=�6    .+�58      �� $	��150��6    $��Сотрудник��6,   �� 59      -
   5        $��
CbEmployee��59   /   $	��150��6#   $��8(�6A    �� DisplayMemberPath$	 FIO��6q   	 �� SelectedItem	 6N   �� *	Employees+6q   5:      � 5;      $ 
Horizontal=�6    .+�5<      �� $	��150��6    $��Возвращен��6,   �� 5=      -   5        
 ��Name$
 
ReturnMark��5=   #    d� 	IsChecked 67   �� *
ReturnMark+5>      5?      5@      � 5B      $��Center=�6   $��35q�65   $ 
Horizontal=�6A   .+�5C      �� -   5        $ Ok��5C      $��Сохранить��6%   69   �� 5D      -   5        $ Cancel��5D      $��Отменить��6)    �� IsCancel . 6<   �� 5E      -   5        $ Delete��5E      $��Удалить��6)   6;   5F      5G      5H      5I      