   M S B A M L   `   `   `  ����  LSTM.UI"    LSTM.UI.Views.RegisterWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  用户注册��5   	   $	��600��5   	   $	��400��5   	    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5	   	      MaterialDesignPaper#���   5
   	     MaterialDesignBody#���  5   	     MaterialDesignFont#���  5   	   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   #  MaterialDesignThemes.Wpf.Card  $��0��6   $��20,20,20,10q�6+   +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�6@   .��5      ��  �� Orientation$ Vertical=�6   $��20q�60   .+�5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ AccountPlus=�6*   $��48��5   (   $��48��5   (   $��Center=�5   (     PrimaryHueMidBrush#���  5   (   �� 5      $��创建新账户��6   $��20(�5      $
��Bold"�5      $��Center=�5       $��0,10,0,0q�5!      5"      5#        5&   
   $��1��6   $��20,10,20,10q�6+   $ Depth2=�6@   .��5'      �� $��30q�6   .+�5)      �� )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 	用户名��5*         
IsFloating$
 True��5+       ! MaterialDesignOutlinedTextBox#��[ 5,      $��0,0,0,20q�5-      ��5)      �� *
Username+	 �� UpdateSourceTrigger$	 PropertyChanged=�5-      �� 50      $ 邮箱地址��51      $
 True��52      #��[ 53      $��0,0,0,20q�54      ��50      �� *Email+$	 PropertyChanged=�54      G� 57      -   5        
 G�Name$
 PasswordBox��57      $ 密码��58      $
 True��59       % !MaterialDesignOutlinedPasswordBox#��[ 5:      $��0,0,0,20q�5;      G� 5>      -   5        $
 ConfirmPasswordBox��5>      $ 确认密码��5?      $
 True��5@      #��[ 5A      $��0,0,0,20q�5B      �� 5E      
���  ��$��12(�5G      $��0,0,0,10q�5H      ��5E      �� *ErrorMessage+y�5I      �� *ErrorMessage+  StringToVisibilityConverter �� 	Converter# [ �� 5L      
��� � �$��12(�5N      $��0,0,0,10q�5O      ��5L      �� *SuccessMessage+y�5P      �� *SuccessMessage+# [ �� 5S      $��注册��6     MaterialDesignRaisedButton#��[ 5U       �� 	IsDefault$
 True��5V      $��40��5W      $��0,10,0,0q�5X      ��5T      �� *RegisterCommand+5X      � 5[       � IsIndeterminate$
 True��6   $��0,10,0,0q�5]      y�5\      �� *	IsLoading+   BooleanToVisibilityConverter# [ 5]      5^      5_      �� 5b   
   $��2��6   $ 
Horizontal=�5c      $��Center=�5d      $��20q�5e      .+�5f      �� $��取消��6     	 MaterialDesignOutlinedButton#��[	 5h      $��80��5i      ��5g      �� *CancelCommand+5i      5j      5k      5l      