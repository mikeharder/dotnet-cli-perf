package ClassLib024;

public class Class001 {
    public static String property() {
        return "ClassLib024" + " " + ClassLib010.Class001.property() + " " + ClassLib013.Class001.property();
    }
}
