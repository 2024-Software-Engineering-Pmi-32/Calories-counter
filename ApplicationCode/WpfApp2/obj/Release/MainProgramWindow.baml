   M S B A M L   `   `   `  ���� U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf   WpfApp2'   !CaloriesCounter.MainProgramWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf       � Title$+  %Калькулятор калорій��$	��600��$	��800�����#F3F4F6 � WindowStartupLocation$ CenterScreen=�$��Segoe UI)� J� Opacity$ 0Q�$��	0,100,0,0q� � Triggers :�  :� RoutedEvent$ Window.Loaded��.[��� .���� .��W�  �� TargetProperty$ Opacity� W� From$ 0X�	 W� To$ 1X� g� Duration$ 0:0:1D�t� $ Margin�	 t� From$	 	0,100,0,0X�	
 t� To$
 0,0,0,0X�$ 0:0:1D� g� FillBehavior$ HoldEnd=�.��� .+�o� $��Stretch=�$	��Top=����#3E50B4
�������$
��Bold"�.@�m� -   $��Прийоми їжі��m� -   $��Статистика��m� -   $/��)Отримати рекомендацію��m� -   $��Нагадування��m� -   $!��Корисні статті���� $	��Top=�$
��Left=�$��	20,60,0,0q�$	��220��.+��� $��images/user_icon.png��$��94��$��94��$��Center=��� -    ��Name$ UserNameTextBlock��$$��Ім'я користувача��$��18(�$��SemiBold"�$��0,10,0,5q�$��Center=����#333�� $%��Ціль користувача��$��16(�$��0,10,0,5q����#555�� -    ��Name$ GoalsComboBox��$	��200��$��0,5,0,10q�$��5q�.@��� $��Скинути вагу���� $'��!Підтримувати вагу���� $��Набрати вагу���� -   $��Зберегти ціль��$	��150��$��35�����#4CAF50
�������$
��Bold"�$��Center=�
������  � Cursor$
 Hand���� $	��Top=�$��Right=�$��	0,60,20,0q�$	��220��.+��� $��Алергени��$��16(�$��0,0,0,5q����#555�� -	    ��Name$ AllergenTextBox��$	��200��$��0,5,0,10q�$��5q����#90A4AE$��1q��� -
   $'��!Зберегти алергени��$	��150��$��35�����#4CAF50
�������$
��Bold"�$��Center=�
������ $
 Hand���� 
������� �� CornerRadius$ 10��$��20,334,40,36q� �� Padding$ 10q�._�#   LiveCharts.Wpf.CartesianChart  -     Name$ caloriesChart��$��-5,-10,-5,-25q�&    LiveCharts.Wpf.Charts.Base.Chart   AxisX    LiveCharts.Wpf.Axis     Title$ Дати��   
Foreground �#333   Labels �� *Dates+   AxisY   $ Калорії�� �#333�� -   $��40��$��40��$��Right=�$	��Top=�$��	0,20,20,0q�
������ 
������ $
 Hand��.���� $��images/settings_icon.png��$��30��$��30��