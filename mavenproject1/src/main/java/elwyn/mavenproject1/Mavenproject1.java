package elwyn.mavenproject1;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.FlowLayout;
import java.sql.*;
import javax.swing.BorderFactory;
import javax.swing.BoxLayout;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTabbedPane;
import javax.swing.JTextArea;

public class Mavenproject1 {

    public static void main(String[] args) {
        JTabbedPane tabbedPane = new JTabbedPane();
        
        JPanel panel1 = new JPanel();
        tabbedPane.addTab("Home", panel1);

        JPanel panel2 = new JPanel();
        panel2.setLayout(new FlowLayout(FlowLayout.CENTER, 10, 10));
        ModelCarView modelCarsView = new ModelCarView(panel2);
        tabbedPane.addTab("Model Cars", panel2);

        JPanel panel3 = new JPanel();
        panel3.setLayout(new FlowLayout(FlowLayout.CENTER, 10, 10));
        ModelCarView ownedView = new OwnedModelCarView(panel3);
        tabbedPane.addTab("Owned", panel3);

        JPanel panel4 = new JPanel();
        panel4.setLayout(new FlowLayout(FlowLayout.CENTER, 10, 10));
        ModelCarView wishedView = new WishedModelCarView(panel4);
        tabbedPane.addTab("Wishlist", panel4);
        
        
        
        JFrame frame = new JFrame();
        frame.add(tabbedPane);
        frame.setExtendedState( frame.getExtendedState()|JFrame.MAXIMIZED_BOTH );
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setVisible(true);
    }
}
