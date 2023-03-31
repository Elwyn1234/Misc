import javax.swing.*;

import java.awt.*;
import java.awt.image.BufferedImage;
import java.awt.image.ImageObserver;
import java.io.File;
import javax.imageio.ImageIO;
import javax.swing.JPanel;

public class MainMenu extends JFrame {
    private JTabbedPane tabbedPane;
    private JPanel rootPanel;
    private JCheckBox checkBox1;
    private JLabel label;
    private JPanel mainMenuPanel;

    MainMenu() {
        try {
            BufferedImage myPicture = ImageIO.read(new File("C:\\home\\dev\\repos\\oop\\assets\\testImage.jpg"));
            label = new JLabel(new ImageIcon(myPicture));
            label.setPreferredSize(new Dimension(100, 100));
            mainMenuPanel.add(label);
            label.prepareImage(myPicture, label);
        } catch (Exception e) {
            System.out.println("error: " + e.toString());
        }
        setContentPane(rootPanel);
        setTitle("Car Model Management");
        setSize(450,300);
        setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);
        setVisible(true);
    }

    public static void main(String[] args) {
        MainMenu mainMenu = new MainMenu();
    }
}
